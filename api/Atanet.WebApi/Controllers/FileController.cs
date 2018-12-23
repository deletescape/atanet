namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Common;

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
