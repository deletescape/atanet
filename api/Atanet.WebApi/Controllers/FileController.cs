namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Files;

    [Route("api/Files")]
    public class FileController : Controller
    {
        private readonly IApiResultService apiResultService;

        private readonly IFileService fileService;

        public FileController(IApiResultService apiResultService, IFileService fileService)
        {
            this.apiResultService = apiResultService;
            this.fileService = fileService;
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000_000)]
        public IActionResult CreateFile(IFormFile file)
        {
            var createdId = this.fileService.CreateFile(file);
            return this.apiResultService.Created(AtanetEntityName.File, createdId);
        }

        [HttpGet("{fileId}")]
        public IActionResult GetFile(long fileId)
        {
            var file = this.fileService.GetFile(fileId);
            return this.File(file.Data, file.ContentType, file.FileName);
        }

        [HttpGet("{fileId}/Info")]
        public IActionResult GetFileInfo(long fileId)
        {
            var fileInfo = this.fileService.GetFileInfo(fileId);
            return this.apiResultService.Ok(fileInfo);
        }
    }
}
