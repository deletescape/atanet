namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Comments;
    using Microsoft.AspNetCore.Authorization;

    [Route("api/posts")]
    public class CreateCommentsController : Controller
    {
        private readonly ICommentCreationService commentCreationService;

        private readonly IApiResultService apiResultService;

        public CreateCommentsController(ICommentCreationService commentCreationService, IApiResultService apiResultService)
        {
            this.commentCreationService = commentCreationService;
            this.apiResultService = apiResultService;
        }

        [HttpPost("{postId}/comments")]
        [Authorize]
        public IActionResult CreateComment([FromRoute] long postId, [FromBody] CreateCommentDto createCommentDto)
        {
            var id = this.commentCreationService.CreateComment(postId, createCommentDto);
            return this.apiResultService.Created(AtanetEntityName.Comment, id);
        }
    }
}
