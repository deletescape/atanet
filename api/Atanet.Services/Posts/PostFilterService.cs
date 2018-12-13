namespace Atanet.Services.Posts
{
    using Microsoft.EntityFrameworkCore;
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.Comments;
    using Atanet.Services.Common;
    using Atanet.Services.Exceptions;
    using Atanet.Services.Files;
    using Atanet.Services.UoW;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Atanet.Services.Scoring;

    public class PostFilterService : IPostFilterService
    {
        private readonly IQueryService queryService;

        private readonly IPagingValidator pagingValidator;

        private readonly ICommentFilterService commentFilterService;

        private readonly IScoreService scoreService;

        public PostFilterService(
            IQueryService queryService,
            IPagingValidator pagingValidator,
            ICommentFilterService commentFilterService,
            IScoreService scoreService)
        {
            this.queryService = queryService;
            this.pagingValidator = pagingValidator;
            this.commentFilterService = commentFilterService;
            this.scoreService = scoreService;
        }

        public IList<PostDto> FilterPosts(int page, int pageSize, int commentCount)
        {
            var enrichedPosts = this.scoreService.GetEnrichedPosts();
            var orderedQuery = enrichedPosts.OrderByDescending(x => x.Score);
            
        }

        private IQueryable<T> Page<T>(IQueryable<T> queryable, int page, int pageSize) =>
            queryable.Skip(page * pageSize).Take(pageSize);
    }
}
