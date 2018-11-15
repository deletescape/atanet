namespace Atanet.Services.Common
{
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Validation;
    using Atanet.Services.Exceptions;
    using System.Collections.Generic;
    using System.Linq;

    public class PagingValidator : IPagingValidator
    {
        public void ThrowIfPageOutOfRange(int pageSize, int page)
        {
            var errors = new Dictionary<ErrorCode, ErrorDefinition>();
            if (pageSize < 0)
            {
                errors.Add(
                    ErrorCode.Parse(ErrorCodeType.OutOfRange, AtanetEntityName.Filter, PropertyName.Filter.PageSize),
                    new ErrorDefinition(pageSize, "Page size must be 0 or bigger", PropertyName.Filter.PageSize));
            }

            if (page < 0)
            {
                errors.Add(
                    ErrorCode.Parse(ErrorCodeType.OutOfRange, AtanetEntityName.Filter, PropertyName.Filter.PageNumber),
                    new ErrorDefinition(page, "Page number cannot be negative", PropertyName.Filter.PageNumber));
            }

            if (errors.Any())
            {
                throw new ApiException(x => x.BadRequestResult(errors));
            }
        }
    }
}
