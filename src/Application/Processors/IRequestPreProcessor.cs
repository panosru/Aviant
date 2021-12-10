namespace Aviant.DDD.Application.Processors;

internal interface IRequestPreProcessor<in TRequest> : MediatR.Pipeline.IRequestPreProcessor<TRequest>
    where TRequest : notnull
{ }
