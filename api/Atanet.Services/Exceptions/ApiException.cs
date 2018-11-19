namespace Atanet.Services.Exceptions
{
    using Atanet.Model.ApiResponse;
    using Atanet.Services.ApiResult;
    using System;

    public class ApiException : Exception
    {
        private static readonly IApiResultService ApiResultService = new ApiResultService();

        public ApiException(Func<IApiResultService, IApiResult> apiResultCreator) =>
            this.ApiResult = apiResultCreator(ApiException.ApiResultService);

        public IApiResult ApiResult { get; set; }
    }
}
