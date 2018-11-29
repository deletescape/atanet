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

    /// <summary>
    /// Accepts all requests to filter posts.
    /// </summary>
    [Route("api/Posts")]
    public class FilterPostsController : Controller
    {
        /// <summary>
        /// Contains the service to filter posts.
        /// </summary>
        private readonly IPostFilterService postFilterService;

        /// <summary>
        /// Contains the service to return formatted JSON results to the client.
        /// </summary>
        private readonly IApiResultService apiResultService;

        private readonly ICommentFilterService commentFilterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterPostsController"/> class.
        /// </summary>
        /// <param name="postFilterService">The service to filter posts.</param>
        /// <param name="apiResultService">The service for formatted API results.</param>
        public FilterPostsController(IPostFilterService postFilterService, IApiResultService apiResultService, ICommentFilterService commentFilterService)
        {
            this.postFilterService = postFilterService;
            this.apiResultService = apiResultService;
            this.commentFilterService = commentFilterService;
        }
    }
}
