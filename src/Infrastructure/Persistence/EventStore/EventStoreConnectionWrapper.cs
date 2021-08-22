namespace Aviant.DDD.Infrastructure.Persistence.EventStore
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::EventStore.ClientAPI;
    using Microsoft.Extensions.Logging;

    public sealed class EventStoreConnectionWrapper : IEventStoreConnectionWrapper, IDisposable
    {
        private readonly Uri _connectionString;

        private readonly Lazy<Task<IEventStoreConnection>> _lazyConnection;

        private readonly ILogger<EventStoreConnectionWrapper> _logger;

        public EventStoreConnectionWrapper(Uri connectionString, ILogger<EventStoreConnectionWrapper> logger)
        {
            _connectionString = connectionString;
            _logger           = logger;

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


            void OnConnected(object? s, ClientConnectionEventArgs e) => _logger.LogInformation(
                $@"Connection established with name '{e.Connection.ConnectionName}' and 
                    address family '{e.RemoteEndPoint.AddressFamily}'.");

            void OnReconnecting(object? s, ClientReconnectingEventArgs e) => _logger.LogInformation($@"Reconnecting to '{e.Connection.ConnectionName}'");

            async void OnErrorOccurred(object? s, ClientErrorEventArgs e)
            {
                _logger.LogWarning(
                    e.Exception,
                    $@"an error has occurred on the EventStore connection: {e
                       .Exception.Message} . Trying to reconnect...");
                connection = SetupConnection();

                await connection.ConnectAsync()
                   .ConfigureAwait(false);
            }

            async void OnDisconnected(object? s, ClientConnectionEventArgs e)
            {
                _logger.LogWarning("The EventStore connection has dropped. Trying to reconnect...");
                connection = SetupConnection();

                await connection.ConnectAsync()
                   .ConfigureAwait(false);
            }

            async void OnClosed(object? s, ClientClosedEventArgs e)
            {
                _logger.LogWarning(
                    $@"The EventStore connection was closed: {e
                       .Reason}. Opening new connection...");
                connection = SetupConnection();

                await connection.ConnectAsync()
                   .ConfigureAwait(false);
            }

            #endregion


            return connection;
        }
    }
}