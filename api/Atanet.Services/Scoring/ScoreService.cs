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
            var postsForUser = this.GetEnrichedPosts(withTimeInCalculation: false)
                .Where(x => x.Post.UserId == userId);
            return postsForUser.Sum(x => x.Score) + difference;
        }

        public IList<UserDto> GetUsersSortedByScore()
        {
            var posts = this.GetEnrichedPosts(withTimeInCalculation: false);
            var usersWithScore =
                from user in this.queryService.Query<User>()
                select new
                {
                    User = user,
                    Score = posts.Where(x => x.Post.UserId == user.Id).Sum(x => x.Score) + (DateTime.Now - user.Created).TotalDays
                };
            return usersWithScore.OrderByDescending(x => x.Score).Select(x => new UserDto
            {
                Email = x.User.Email,
                Id = x.User.Id
            }).ToList();
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
    }
}
