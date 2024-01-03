using MediatR;

namespace Aviant.Application.Processors;

internal interface IRequestPostProcessor<in TRequest, in TResponse>
    : MediatR.Pipeline.IRequestPostProcessor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{ }
