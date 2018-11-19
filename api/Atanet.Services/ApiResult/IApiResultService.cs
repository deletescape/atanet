namespace Atanet.Services.ApiResult
{
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.ApiResponse;
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Validation;
    using System;
    using System.Collections.Generic;

    public interface IApiResultService
    {
        IApiResult BadRequestResult(IdentityResult identityResult, object attemptedValue, PropertyName propertyName);

        IApiResult NoContentResult();

        IApiResult OkResult(object obj);
        
        IApiResult OkResult();

        IApiResult CreatedResult(AtanetEntityName entity, long id);

        IApiResult BadRequestResult(IDictionary<ErrorCode, ErrorDefinition> errors);

        IActionResult BadRequest(params (ErrorCode code, ErrorDefinition definition)[] errors);

        IApiResult BadRequestResult(params (ErrorCode code, ErrorDefinition definition)[] errors);

        IApiResult BadRequestResult(ValidationResult validationResult);

        IApiResult BadRequestResult(IEnumerable<ValidationResult> validationResult);

        IApiResult UnauthorizedResult();

        IApiResult ForbiddenResult(AtanetEntityName accessedEntity, long accessedEntityId);

        IApiResult NotFoundResult(AtanetEntityName accessedEntity, long accessedEntityId);

        IApiResult InternalServerErrorResult(Exception ex);

        IActionResult NoContent();

        IActionResult Ok(object obj);

        IActionResult Ok();

        IActionResult Created(AtanetEntityName entity, long id);

        IActionResult BadRequest(IDictionary<ErrorCode, ErrorDefinition> errors);

        IActionResult BadRequest(ValidationResult validationResult);

        IActionResult BadRequest(IEnumerable<ValidationResult> validationResult);

        IActionResult Unauthorized();

        IActionResult Forbidden(AtanetEntityName accessedEntity, long accessedEntityId);

        IActionResult NotFound(AtanetEntityName accessedEntity, long accessedEntityId);

        IActionResult InternalServerError(Exception ex);
    }
}
