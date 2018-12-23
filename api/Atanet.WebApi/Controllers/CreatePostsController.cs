namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Model.Dto;
    using Model.Validation;
    using Services.ApiResult;
    using Services.Posts;

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
