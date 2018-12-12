namespace Atanet.Services.Scoring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Exceptions;
    using Atanet.Services.UoW;
    using Microsoft.EntityFrameworkCore;

    public class ScoreService : IScoreService
    {
        private readonly Dictionary<AtanetAction, long> minScoresMap = new Dictionary<AtanetAction, long>
        {
            { AtanetAction.DeleteLowScoreUser, 200 },
            { AtanetAction.CreateComment, 75 },
            { AtanetAction.ViewUserProfile, 50 },
            { AtanetAction.CreatePost, 0 },
            { AtanetAction.VotePost, -10 },
            { AtanetAction.ViewOwnUserProfile, -100 }
        };

        private readonly IQueryService queryService;

        private readonly IApiResultService apiResultService;

        private readonly long PostConstant = 20;

        public ScoreService(IQueryService queryService, IApiResultService apiResultService)
        {
            this.queryService = queryService;
            this.apiResultService = apiResultService;
        }

        public double CalculateUserScore(long userId)
        {
            var userCreationDate = this.queryService.Query<User>().Where(x => x.Id == userId).Select(x => x.Created).FirstOrDefault();
            if (userCreationDate == default(DateTime))
            {
                throw new ApiException(this.apiResultService.NotFoundResult(AtanetEntityName.User, userId));
            }

            var difference = (DateTime.Now - userCreationDate).TotalDays;
            var postScoreForUser = this.GetEnrichedPosts().Where(x => x.Post.UserId == userId).Sum(x => x.Score);
            return postScoreForUser + difference;
        }

        public IQueryable<PostWithScoreDto> GetEnrichedPosts()
        {
            var enrichedPosts = 
                from post in this.queryService.Query<Post>()
                join reaction in this.queryService.Query<PostReaction>() on post.Id equals reaction.PostId into reactions
                select new
                {
                    Post = post,
                    // Formulas as defined in Atanet specification
                    InitialPostScore = (post.Sentiment - 0.5) * ((-(PostConstant / 2)) * Math.Pow(1.5 + Math.Abs(post.Sentiment - 0.5), 3)),
                    Reactions = reactions
                };
            return enrichedPosts.Select(x => new PostWithScoreDto
            {
                Post = x.Post,
                Score = x.Reactions.Sum(p => (int)p.ReactionState) - (DateTime.Now - x.Post.Created).TotalDays + x.InitialPostScore
            });
        }

        public bool Can(AtanetAction action, long userId)
        {
            var score = this.CalculateUserScore(userId);
            var minScoreForAction = this.minScoresMap[action];
            return score > minScoreForAction;
        }

        public long GetMinScore(AtanetAction action) =>
            this.minScoresMap[action];
    }
}
