namespace Atanet.Services.ApiResult
{
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.ApiResponse;
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Validation;
    using System;
    using System.Collections.Generic;

    public interface IApiResultService
    {
        IApiResult OkResult(object obj);

        IApiResult OkResult();

        IApiResult CreatedResult(AtanetEntityName entity, long id);

        IApiResult BadRequestResult(IDictionary<ErrorCode, ErrorDefinition> errors);

        IApiResult BadRequestResult(params (ErrorCode code, ErrorDefinition definition)[] errors);

        IApiResult BadRequestResult(string message);

        IApiResult BadRequestResult(ValidationResult validationResult);

        IApiResult BadRequestResult(IEnumerable<ValidationResult> validationResult);

        IApiResult UnauthorizedResult();

        IApiResult NotFoundResult(AtanetEntityName accessedEntity, long accessedEntityId);

        IApiResult InternalServerErrorResult(Exception ex);

        IActionResult Ok(object obj);

        IActionResult Ok(string message);

        IActionResult Created(AtanetEntityName entity, long id);

        IActionResult BadRequest(IEnumerable<ValidationResult> validationResult);

        IActionResult InternalServerError(Exception ex);
    }
}
