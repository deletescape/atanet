namespace Atanet.Services.Exceptions
{
    using Atanet.Model.ApiResponse;
    using Atanet.Services.ApiResult;
    using System;

    public class ApiException : Exception
    {
        public ApiException(IApiResult apiResult) =>
            this.ApiResult = apiResult;

        public IApiResult ApiResult { get; set; }
    }
}
