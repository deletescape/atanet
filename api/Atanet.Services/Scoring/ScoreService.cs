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
            var userDto = this.GetUsersByScoreQuery().FirstOrDefault(x => x.Id == userId);
            if (userDto == null)
            {
                throw new ApiException(this.apiResultService.NotFoundResult(AtanetEntityName.User, userId));
            }

            return userDto.Score;
        }

        public IList<UserWithScoreDto> GetUsersSortedByScore()
        {
            var topEntries = int.Parse(Environment.GetEnvironmentVariable("SCOREBOARD_ENTRIES"));
            return this.GetUsersByScoreQuery()
                .OrderByDescending(x => x.Score)
                .Take(topEntries).ToList();
        }

        public IQueryable<PostWithScoreDto> GetEnrichedPosts(bool withTimeInCalculation)
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
                Score = x.Reactions.Sum(p => (int)p.ReactionState) - (withTimeInCalculation ? (DateTime.Now - x.Post.Created).TotalDays : 0) + x.InitialPostScore
            });
        }

        public bool Can(AtanetAction action, long userId)
        {
            var score = this.CalculateUserScore(userId);
            var minScoreForAction = this.GetMinScore(action);
            return score > minScoreForAction;
        }

        public long GetMinScore(AtanetAction action) =>
            this.minScoresMap[action];

        public IEnumerable<AtanetAction> GetUserCapabilities(long userId)
        {
            var score = this.CalculateUserScore(userId);
            foreach (var entry in this.minScoresMap)
            {
                if (entry.Value <= score)
                {
                    yield return entry.Key;
                }
            }
        }

        private IQueryable<UserWithScoreDto> GetUsersByScoreQuery()
        {
            var posts = this.GetEnrichedPosts(withTimeInCalculation: false);
            var result =
                from user in this.queryService.Query<User>()
                join post in posts on user.Id equals post.Post.UserId into userPosts
                select new UserWithScoreDto
                {
                    Created = user.Created,
                    Email = user.Email,
                    Id = user.Id,
                    Score = userPosts.Sum(x => x.Score) + (DateTime.Now - user.Created).TotalDays
                };
            return result;
        }
    }
}
