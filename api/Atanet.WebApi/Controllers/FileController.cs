namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Files;
    using Microsoft.AspNetCore.Authorization;
    using Atanet.Services.Common;

    [Route("api/files")]
    public class FileController : Controller
    {
        private readonly IPictureService pictureService;

        public FileController(IPictureService pictureService)
        {
            this.pictureService = pictureService;
        }

        [AllowAnonymous]
        [HttpGet("expression")]
        public IActionResult Expression() =>
            Redirect(this.pictureService.GetPictureUrl());
    }
}
