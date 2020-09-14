namespace Aviant.DDD.Core.EventBus
{
    #region

    using System;
    using System.Threading.Tasks;
    using Aggregates;

    #endregion

    public interface IEventProducer<in TAggregate, in TAggregateId> : IDisposable
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        Task DispatchAsync(TAggregate aggregate);
    }
}