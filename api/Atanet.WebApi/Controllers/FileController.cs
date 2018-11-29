namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Files;

    [Route("api/files")]
    public class FileController : Controller
    {
        private readonly IApiResultService apiResultService;

        public FileController(IApiResultService apiResultService)
        {
            this.apiResultService = apiResultService;
        }
    }
}
