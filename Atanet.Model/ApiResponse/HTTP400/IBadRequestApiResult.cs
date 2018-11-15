namespace Atanet.Model.ApiResponse.HTTP400
{
    using Atanet.Model.Validation;
    using System.Collections.Generic;

    public interface IBadRequestApiResult : IApiResult
    {
        IDictionary<ErrorCode, ErrorDefinition> Errors { get; set; }
    }
}
