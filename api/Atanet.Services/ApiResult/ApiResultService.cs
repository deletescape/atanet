namespace Atanet.Services.ApiResult
{
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.ApiResponse;
    using Atanet.Model.ApiResponse.HTTP200;
    using Atanet.Model.ApiResponse.HTTP201;
    using Atanet.Model.ApiResponse.HTTP204;
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.ApiResponse.HTTP401;
    using Atanet.Model.ApiResponse.HTTP403;
    using Atanet.Model.ApiResponse.HTTP404;
    using Atanet.Model.ApiResponse.HTTP500;
    using Atanet.Model.Validation;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ApiResultService : IApiResultService
    {
        public IApiResult BadRequestResult(IdentityResult identityResult, object attemptedValue, PropertyName propertyName)
        {
            if (identityResult.Succeeded)
            {
                return this.OkResult();
            }

            var errors = identityResult.Errors.ToDictionary(
                x => ErrorCode.Parse(x.Code),
                x => new ErrorDefinition(attemptedValue, x.Description, propertyName));
            var result = this.BadRequestResult(errors);
            return result;
        }

        public IActionResult BadRequest(IDictionary<ErrorCode, ErrorDefinition> errors) => 
            this.BadRequestResult(errors).GetResultObject();

        public IActionResult BadRequest(ValidationResult validationResult) =>
            this.BadRequestResult(validationResult).GetResultObject();

        public IActionResult BadRequest(IEnumerable<ValidationResult> validationResult) =>
            this.BadRequestResult(validationResult).GetResultObject();

        public IApiResult NoContentResult() =>
            new NoContentApiResult();

        public IApiResult BadRequestResult(IDictionary<ErrorCode, ErrorDefinition> errors) =>
            new BadRequestApiResult(errors);

        public IApiResult BadRequestResult(ValidationResult validationResult)
        {
            var errors = this.GetErrorDefinitionsFromValidationResult(validationResult);
            return new BadRequestApiResult(errors);
        }

        public IApiResult BadRequestResult(IEnumerable<ValidationResult> validationResults)
        {
            var validations = new Dictionary<ErrorCode, ErrorDefinition>();
            foreach (var validationResult in validationResults)
            {
                var errors = this.GetErrorDefinitionsFromValidationResult(validationResult);
                foreach (var item in errors)
                {
                    validations.Add(item.Key, item.Value);
                }
            }

            return new BadRequestApiResult(validations);
        }

        public IActionResult BadRequest(params (ErrorCode code, ErrorDefinition definition)[] errors) =>
            this.BadRequestResult(errors).GetResultObject();

        public IApiResult BadRequestResult(params (ErrorCode code, ErrorDefinition definition)[] errors) =>
            this.BadRequestResult(errors.ToDictionary(x => x.code, x => x.definition));

        public IActionResult Created(AtanetEntityName entity, long id) =>
            this.CreatedResult(entity, id).GetResultObject();

        public IApiResult CreatedResult(AtanetEntityName entity, long id) =>
            new CreatedApiResult(entity, id);

        public IActionResult Forbidden(AtanetEntityName accessedEntity, long accessedEntityId) =>
            this.ForbiddenResult(accessedEntity, accessedEntityId).GetResultObject();

        public IApiResult ForbiddenResult(AtanetEntityName accessedEntity, long accessedEntityId)
        {
            try
            {
                return new ForbiddenApiResult(-1, accessedEntity, accessedEntityId);
            }
            catch (InvalidCastException)
            {
                return this.UnauthorizedResult();
            }
        }

        public IActionResult InternalServerError(Exception ex) => 
            this.InternalServerErrorResult(ex).GetResultObject();

        public IApiResult InternalServerErrorResult(Exception ex) =>
            new InternalServerErrorApiResult(ex);

        public IActionResult NotFound(AtanetEntityName accessedEntity, long accessedEntityId) =>
            this.NotFoundResult(accessedEntity, accessedEntityId).GetResultObject();

        public IApiResult NotFoundResult(AtanetEntityName accessedEntity, long accessedEntityId) => 
            new NotFoundApiResult(accessedEntity, accessedEntityId);

        public IActionResult Ok(object obj) =>
            this.OkResult(obj).GetResultObject();

        public IApiResult OkResult(object obj) => 
            new OkApiResult(obj);

        public IActionResult Unauthorized() =>
            this.UnauthorizedResult().GetResultObject();

        public IApiResult UnauthorizedResult() =>
            new UnauthorizedApiResult();

        public IApiResult OkResult() =>
            new OkApiResult(null);

        public IActionResult Ok() =>
            this.OkResult().GetResultObject();

        public IActionResult NoContent() =>
            this.NoContentResult().GetResultObject();

        private IDictionary<ErrorCode, ErrorDefinition> GetErrorDefinitionsFromValidationResult(ValidationResult result)
        {
            var dic = new Dictionary<ErrorCode, ErrorDefinition>();
            foreach (var validationResult in result.Errors)
            {
                var errorCode = ErrorCode.Parse(validationResult.ErrorCode);
                dic.Add(
                    errorCode,
                    new ErrorDefinition(
                        validationResult.AttemptedValue,
                        validationResult.ErrorMessage,
                        PropertyName.Parse(validationResult.PropertyName)));
            }

            return dic;
        }
    }
}
