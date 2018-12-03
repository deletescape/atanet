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

    [Route("api/posts")]
    public class FilterPostsController : Controller
    {
        private readonly IPostFilterService postFilterService;

        private readonly IApiResultService apiResultService;

        private readonly ICommentFilterService commentFilterService;

        public FilterPostsController(IPostFilterService postFilterService, IApiResultService apiResultService, ICommentFilterService commentFilterService)
        {
            this.postFilterService = postFilterService;
            this.apiResultService = apiResultService;
            this.commentFilterService = commentFilterService;
        }
    }
}
