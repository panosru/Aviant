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

            connection.ErrorOccurred += async (s, e) =>
            {
                _logger.LogWarning(
                    e.Exception,
                    $@"an error has occurred on the EventStore connection: {e
                       .Exception.Message} . Trying to reconnect...");
                connection = SetupConnection();

                await connection.ConnectAsync()
                   .ConfigureAwait(false);
            };

            connection.Disconnected += async (s, e) =>
            {
                _logger.LogWarning("The EventStore connection has dropped. Trying to reconnect...");
                connection = SetupConnection();

                await connection.ConnectAsync()
                   .ConfigureAwait(false);
            };

            connection.Closed += async (s, e) =>
            {
                _logger.LogWarning(
                    $@"The EventStore connection was closed: {e
                       .Reason}. Opening new connection...");
                connection = SetupConnection();

                await connection.ConnectAsync()
                   .ConfigureAwait(false);
            };

            return connection;
        }
    }
}