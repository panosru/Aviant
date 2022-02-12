namespace Aviant.Foundation.Application.Processors;

internal interface IRequestPreProcessor<in TRequest> : MediatR.Pipeline.IRequestPreProcessor<TRequest>
    where TRequest : notnull
{ }
