namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Model.Dto;
    using Model.Validation;
    using Services.ApiResult;
    using Services.Posts.Reactions;

    [Route("api/posts")]
    public class PostReactionController : Controller
    {
        private readonly IApiResultService apiResultService;

        private readonly IPostReactionCreationService postReactionCreationService;

        public PostReactionController(IApiResultService apiResultService, IPostReactionCreationService postReactionCreationService)
        {
            this.apiResultService = apiResultService;
            this.postReactionCreationService = postReactionCreationService;
        }

        [Authorize]
        [HttpPost("{postId}/reactions")]
        public IActionResult AddPostReaction(long postId, [FromBody] CreateReactionDto createReactionDto)
        {
            var createdId = this.postReactionCreationService.AddReaction(postId, createReactionDto);
            return this.apiResultService.Created(AtanetEntityName.Reaction, createdId);
        }
    }
}
