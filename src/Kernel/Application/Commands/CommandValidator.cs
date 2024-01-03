using FluentValidation;

namespace Aviant.Application.Commands;

public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
{
    protected CommandValidator(CascadeMode cascadeMode = CascadeMode.Stop) => ClassLevelCascadeMode = cascadeMode;
}
