namespace Atanet.Services.BusinessRules
{
    using FluentValidation;
    using Atanet.Services.BusinessRules.Interfaces;
    using Atanet.Services.Exceptions;
    using Atanet.Services.UoW;
    using System.Collections.Generic;
    using System.Linq;

    public class ValidationBusinessRule : IBusinessRuleBase
    {
        private readonly IValidatorFactory validatorFactory;

        public ValidationBusinessRule(IValidatorFactory validatorFactory) =>
            this.validatorFactory = validatorFactory;

        public void PostSave(IUnitOfWork unitOfWork)
        {
        }

        public void PreSave(IList<object> added, IList<object> updated, IList<object> removed)
        {
            foreach (var item in added.Concat(updated))
            {
                this.ValidateObjectAndThrow(item);
            }
        }

        private void ValidateObjectAndThrow(object obj)
        {
            var validator = this.validatorFactory.GetValidator(obj.GetType());
            if (validator == null)
            {
                return;
            }

            var validationResult = validator.Validate(obj);
            if (!validationResult.IsValid)
            {
                throw new ApiException(x => x.BadRequestResult(validationResult));
            }
        }
    }
}
