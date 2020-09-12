namespace Aviant.DDD.Application.Commands
{
    #region

    using FluentValidation;

    #endregion

    public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
    {
        protected CommandValidator(CascadeMode cascadeMode = CascadeMode.Stop) => CascadeMode = cascadeMode;
    }
}