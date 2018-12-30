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
    using AutoMapper;
    using Atanet.Services.ApiResult;

    public class PostFilterService : IPostFilterService
    {
        private readonly IQueryService queryService;

        private readonly IPagingValidator pagingValidator;

        private readonly ICommentFilterService commentFilterService;

        private readonly IScoreService scoreService;

        private readonly IMapper mapper;

        private readonly IApiResultService apiResultService;

        public PostFilterService(
            IQueryService queryService,
            IPagingValidator pagingValidator,
            ICommentFilterService commentFilterService,
            IScoreService scoreService,
            IMapper mapper,
            IApiResultService apiResultService)
        {
            this.queryService = queryService;
            this.pagingValidator = pagingValidator;
            this.commentFilterService = commentFilterService;
            this.scoreService = scoreService;
            this.mapper = mapper;
            this.apiResultService = apiResultService;
        }

        public File GetPictureForPost(long postId)
        {
            if (!this.queryService.Query<Post>().Any(x => x.Id == postId))
            {
                throw new ApiException(this.apiResultService.NotFoundResult(AtanetEntityName.Post, postId));
            }

            return this.queryService.Query<Post>()
                .Include(x => x.Picture)
                .First(x => x.Id == postId)
                .Picture;
        }

        public IList<PostDto> FilterPosts(int page, int pageSize, int commentCount)
        {
            this.pagingValidator.ThrowIfPageOutOfRange(pageSize, page);
            var enrichedPosts = this.scoreService.GetEnrichedPosts(withTimeInCalculation: true);
            var orderedQuery = enrichedPosts.OrderByDescending(x => x.Score);
            var fetchedPage = orderedQuery.Skip(pageSize * page).Take(pageSize);
            var results =
                from post in fetchedPage
                join user in this.queryService.Query<User>().Include(x => x.Picture) on post.Post.UserId equals user.Id
                join reaction in this.queryService.Query<PostReaction>() on post.Post.Id equals reaction.PostId into reactions
                join comment in this.queryService.Query<Comment>().Include(x => x.User.Picture) on post.Post.Id equals comment.PostId into comments
                join postPicture in this.queryService.Query<File>() on post.Post.PictureId equals postPicture.Id
                select new PostDto
                {
                    Created = post.Post.Created,
                    Score = post.Score,
                    Id = post.Post.Id,
                    Reactions = reactions.GroupBy(x => x.ReactionState).ToDictionary(x => x.Key, x => x.Count()),
                    Text = post.Post.Text,
                    Picture = new PictureDto
                    {
                        Base64Data = Convert.ToBase64String(postPicture.Data),
                        ContentType = postPicture.ContentType
                    },
                    User = new UserDto
                    {
                        Email = user.Email,
                        Id = user.Id,
                        Picture = new PictureDto
                        {
                            Base64Data = Convert.ToBase64String(user.Picture.Data),
                            ContentType = user.Picture.ContentType
                        }
                    },
                    Comments = comments.Select(x => new CommentDto
                    {
                        Created = x.Created,
                        Id = x.Id,
                        PostId = x.PostId,
                        Text = x.Text,
                        User = new UserDto
                        {
                            Email = x.User.Email,
                            Id = x.UserId,
                            Picture = new PictureDto
                            {
                                Base64Data = Convert.ToBase64String(x.User.Picture.Data),
                                ContentType = x.User.Picture.ContentType
                            }
                        }
                    }).OrderByDescending(x => x.Created).Take(commentCount).ToArray()
                };
            return results.ToList();
        }

        private IQueryable<T> Page<T>(IQueryable<T> queryable, int page, int pageSize) =>
            queryable.Skip(page * pageSize).Take(pageSize);
    }
}
