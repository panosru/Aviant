namespace Aviant.Foundation.Application.Processors;

using MediatR;

internal interface IRequestPostProcessor<in TRequest, in TResponse>
    : MediatR.Pipeline.IRequestPostProcessor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{ }
