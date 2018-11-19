namespace Atanet.Validation
{
    using FluentValidation;

    public abstract class AbstractAtanetValidator<T> : AbstractValidator<T>
    {
        public AbstractAtanetValidator() =>
            this.Initalize();

        protected abstract void Initalize();
    }
}
