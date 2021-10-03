namespace Aviant.DDD.Application.Processors
{
    internal interface IRequestPostProcessor<in TRequest, in TResponse>
        : MediatR.Pipeline.IRequestPostProcessor<TRequest, TResponse>
        where TRequest : notnull
    { }
}
