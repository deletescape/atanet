namespace Atanet.WebApi.Infrastructure
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Services.ApiResult;
    using Services.Exceptions;

    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IApiResultService result;

        public GlobalExceptionFilter() =>
            this.result = new ApiResultService();

        public void OnException(ExceptionContext context)
        {
            var innerMost = context.Exception;
            while (innerMost.InnerException != null)
            {
                innerMost = innerMost.InnerException;
            }

            if (context.ExceptionHandled)
            {
                return;
            }

            void SetException(ApiException exception)
            {
                var apiResult = exception.ApiResult;
                context.Result = apiResult.GetResultObject();
            }

            if (context.Exception is ApiException ex)
            {
                SetException(ex);
                return;
            }

            if (innerMost is ApiException innerMostApiException)
            {
                SetException(innerMostApiException);
                return;
            }

            context.Result = this.result.InternalServerError(context.Exception);
        }
    }
}
