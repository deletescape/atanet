namespace Atanet.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Model.Dto;
    using Services.ApiResult;
    using Services.Comments;
    using Services.Posts;

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

        [HttpGet("{postId}/picture")]
        public IActionResult PostPicture(long postId)
        {
            var file = this.postFilterService.GetPictureForPost(postId);
            return File(file.Data, file.ContentType);
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
