namespace Aviant.DDD.Application.Commands
{
    using MediatR;

    public abstract class Command<TResponse> : ICommand<TResponse>
    {
    }

    public abstract class Command : Command<Unit>, ICommand
    {
    }
}