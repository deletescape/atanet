namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.Dto;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Comments;
    using Atanet.Services.Posts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Atanet.Services.Posts.Reactions;
    using Atanet.Model.Validation;

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
