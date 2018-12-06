namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Posts;
    using Microsoft.AspNetCore.Authorization;

    [Route("api/posts")]
    public class CreatePostsController : Controller
    {
        private readonly IPostCreationService postCreationService;

        private readonly IApiResultService apiResultService;

        public CreatePostsController(IPostCreationService postCreationService, IApiResultService apiResultService)
        {
            this.postCreationService = postCreationService;
            this.apiResultService = apiResultService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreatePost([FromForm] CreatePostDto createPostDto)
        {
            var id = this.postCreationService.CreatePost(createPostDto);
            return this.apiResultService.Created(AtanetEntityName.Post, id);
        }
    }
}
