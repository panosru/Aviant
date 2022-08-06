namespace Aviant.Application.Commands;

using FluentValidation;

public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
{
    protected CommandValidator(CascadeMode cascadeMode = CascadeMode.Stop) => ClassLevelCascadeMode = cascadeMode;
}
