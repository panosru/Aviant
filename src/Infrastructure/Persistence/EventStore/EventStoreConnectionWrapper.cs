namespace Aviant.DDD.Infrastructure.Persistence.EventStore
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::EventStore.ClientAPI;
    using Serilog;

    public sealed class EventStoreConnectionWrapper : IEventStoreConnectionWrapper, IDisposable
    {
        private readonly Uri _connectionString;

        private readonly Lazy<Task<IEventStoreConnection>> _lazyConnection;

        private readonly Serilog.ILogger _logger = Log.Logger.ForContext<EventStoreConnectionWrapper>();

        public EventStoreConnectionWrapper(Uri connectionString)
        {
            _connectionString = connectionString;

            _lazyConnection = new Lazy<Task<IEventStoreConnection>>(
                () => Task.Run(
                    async () =>
                    {
                        var connection = SetupConnection();

                        await connection.ConnectAsync()
                           .ConfigureAwait(false);

                        return connection;
                    }));
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!_lazyConnection.IsValueCreated)
                return;

            _lazyConnection.Value.Result.Dispose();
        }

        #endregion

        #region IEventStoreConnectionWrapper Members

        public Task<IEventStoreConnection> GetConnectionAsync(CancellationToken cancellationToken = default) =>
            _lazyConnection.Value;

        #endregion

        private IEventStoreConnection SetupConnection()
        {
            var settings = ConnectionSettings.Create()
                // .EnableVerboseLogging()
               .UseConsoleLogger()
               .DisableTls() // https://github.com/EventStore/EventStore/issues/2547
               .KeepReconnecting()
               .LimitReconnectionsTo(10)
               .Build();

            var connection = EventStoreConnection.Create(settings, _connectionString);

            #region Connection Events

            connection.Connected     += OnConnected;
            connection.Reconnecting  += OnReconnecting;
            connection.ErrorOccurred += OnErrorOccurred;
            connection.Disconnected  += OnDisconnected;
            connection.Closed        += OnClosed;


            void OnConnected(object? s, ClientConnectionEventArgs e) => _logger.Information(
                "Connection established with name '{ConnectionName}' and address family '{AddressFamily}'",
                e.Connection.ConnectionName,
                e.RemoteEndPoint.AddressFamily);

            void OnReconnecting(object? s, ClientReconnectingEventArgs e) =>
                _logger.Information("Reconnecting to '{ConnectionName}'", e.Connection.ConnectionName);

            async void OnErrorOccurred(object? s, ClientErrorEventArgs e)
            {
                _logger.Warning(
                    e.Exception,
                    "an error has occurred on the EventStore connection: {Message} . Trying to reconnect...",
                    e.Exception.Message);
                connection = SetupConnection();

                await connection.ConnectAsync()
                   .ConfigureAwait(false);
            }

            async void OnDisconnected(object? s, ClientConnectionEventArgs e)
            {
                _logger.Warning("The EventStore connection has dropped. Trying to reconnect...");
                connection = SetupConnection();

                await connection.ConnectAsync()
                   .ConfigureAwait(false);
            }

            async void OnClosed(object? s, ClientClosedEventArgs e)
            {
                _logger.Warning(
                    "The EventStore connection was closed: {Reason}. Opening new connection...",
                    e.Reason);
                connection = SetupConnection();

                await connection.ConnectAsync()
                   .ConfigureAwait(false);
            }

            #endregion


            return connection;
        }
    }
}
