namespace Atanet.Services.Exceptions
{
    using Atanet.Model.ApiResponse;
    using Atanet.Services.ApiResult;
    using System;

    public class ApiException : Exception
    {
        private static readonly IApiResultService ApiResultService = new ApiResultService();

        // TODO: always pass reuslt in here instead of making the exception
        // provide the result service
        public ApiException(Func<IApiResultService, IApiResult> apiResultCreator) =>
            this.ApiResult = apiResultCreator(ApiException.ApiResultService);

        public IApiResult ApiResult { get; set; }
    }
}
