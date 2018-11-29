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

    public class PostFilterService : IPostFilterService
    {
        private readonly IQueryService queryService;

        private readonly IPagingValidator pagingValidator;

        private readonly ICommentFilterService commentFilterService;

        private readonly IFileService fileService;

        public PostFilterService(
            IQueryService queryService,
            IPagingValidator pagingValidator,
            ICommentFilterService commentFilterService,
            IFileService fileService)
        {
            this.queryService = queryService;
            this.pagingValidator = pagingValidator;
            this.commentFilterService = commentFilterService;
            this.fileService = fileService;
        }

        private IQueryable<T> Page<T>(IQueryable<T> queryable, int page, int pageSize) =>
            queryable.Skip(page * pageSize).Take(pageSize);
    }
}
