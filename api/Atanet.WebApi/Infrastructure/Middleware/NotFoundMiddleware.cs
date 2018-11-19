namespace Atanet.WebApi.Infrastructure.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Atanet.Model.Validation;
    using Atanet.Services;
    using Atanet.Services.ApiResult;
    using System.Threading.Tasks;

    public class NotFoundMiddleware
    {
        private readonly RequestDelegate next;

        public NotFoundMiddleware(RequestDelegate next) =>
            this.next = next;

        public async Task Invoke(HttpContext context)
        {
            await this.next(context);
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var apiResultService = ServiceLocator.Instance.GetService<IApiResultService>();
                context.Response.ContentType = "application/json";
                var result = apiResultService.NotFoundResult(AtanetEntityName.Unspecified, -1);
                result.Message = "Route not found";
                var notFound = result.GetResultObject() as JsonResult;
                var json = JsonConvert.SerializeObject(
                    notFound.Value, 
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                await context.Response.WriteAsync(json);
                return;
            }
        }
    }
}
