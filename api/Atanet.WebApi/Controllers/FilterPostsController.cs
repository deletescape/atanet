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

    [Route("api/posts")]
    public class FilterPostsController : Controller
    {
        private readonly IPostFilterService postFilterService;

        private readonly ICommentFilterService commentFilterService;

        private readonly IApiResultService apiResultService;

        public FilterPostsController(
            IPostFilterService postFilterService,
            IApiResultService apiResultService,
            ICommentFilterService commentFilterService)
        {
            this.postFilterService = postFilterService;
            this.apiResultService = apiResultService;
            this.commentFilterService = commentFilterService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult FilterPosts(PagedPostDto pagedPostDto)
        {
            var posts = this.postFilterService.FilterPosts(pagedPostDto.PageNumber, pagedPostDto.PageSize, pagedPostDto.CommentNumber);
            return this.apiResultService.Ok(posts);
        }

        [HttpGet("{postId}/comments")]
        [Authorize]
        public IActionResult FilterComments(long postId, PagedDto pagedDto)
        {
            var comments = this.commentFilterService.GetCommentsForPost(postId, pagedDto.PageNumber, pagedDto.PageSize);
            return this.apiResultService.Ok(comments);
        }
    }
}
