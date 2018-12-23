namespace Atanet.WebApi.Infrastructure.Middleware
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Model.Validation;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Services.ApiResult;

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
                var apiResultService = context.RequestServices.GetService<IApiResultService>();
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
            }
        }
    }
}
