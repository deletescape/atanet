namespace Atanet.Services.BusinessRules
{
    using FluentValidation;
    using Atanet.Services.BusinessRules.Interfaces;
    using Atanet.Services.Exceptions;
    using Atanet.Services.UoW;
    using System.Collections.Generic;
    using System.Linq;
    using Atanet.Services.ApiResult;

    public class ValidationBusinessRule : IBusinessRuleBase
    {
        private readonly IValidatorFactory validatorFactory;

        private readonly IApiResultService apiResultService;

        public ValidationBusinessRule(IValidatorFactory validatorFactory, IApiResultService apiResultService)
        {
            this.validatorFactory = validatorFactory;
            this.apiResultService = apiResultService;
        }

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
                throw new ApiException(this.apiResultService.BadRequestResult(validationResult));
            }
        }
    }
}
