namespace Atanet.Services.Posts.Reactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Validation;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Authentication;
    using Atanet.Services.Exceptions;
    using Atanet.Services.Scoring;
    using Atanet.Services.UoW;

    public class PostReactionCreationService : IPostReactionCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IQueryService queryService;

        private readonly IApiResultService apiResultService;

        private readonly IUserService userService;

        private readonly IScoreService scoreService;

        public PostReactionCreationService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IQueryService queryService,
            IApiResultService apiResultService,
            IUserService userService,
            IScoreService scoreService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.queryService = queryService;
            this.apiResultService = apiResultService;
            this.userService = userService;
            this.scoreService = scoreService;
        }

        public IDictionary<ReactionState, int> AddReaction(long postId, CreateReactionDto createReactionDto)
        {
            if (!this.queryService.Query<Post>().Any(x => x.Id == postId))
            {
                throw new ApiException(this.apiResultService.NotFoundResult(AtanetEntityName.Post, postId));
            }

            var currentUserId = this.userService.GetCurrentUserId();
            if (!this.scoreService.Can(AtanetAction.VotePost, currentUserId))
            {
                var minScore = this.scoreService.GetMinScore(AtanetAction.VotePost);
                throw new ApiException(this.apiResultService.BadRequestResult($"Must have a score greater than {minScore} in order to vote for posts"));
            }

            var postCreator = this.queryService.Query<Post>().Where(x => x.Id == postId).Select(x => x.UserId).Single();
            if (postCreator == currentUserId)
            {
                throw new ApiException(this.apiResultService.BadRequestResult("You cannot react on a post you have authored"));
            }

            var state = this.GetReactionState(createReactionDto.ReactionState.Value);
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var postReactionRepository = unitOfWork.CreateEntityRepository<PostReaction>();
                var existing = postReactionRepository.Query().FirstOrDefault(x => x.PostId == postId && x.UserId == currentUserId);
                if (existing == null)
                {
                    postReactionRepository.Create(new PostReaction
                    {
                        PostId = postId,
                        ReactionState = state,
                        UserId = currentUserId
                    });
                }
                else
                {
                    existing.ReactionState = state;
                    postReactionRepository.Update(existing);
                }

                unitOfWork.Save();
            }

            var reactions = this.queryService.Query<PostReaction>().Where(x => x.PostId == postId)
                .GroupBy(x => x.ReactionState).ToDictionary(x => x.Key, x => x.Count());

            return reactions;
        }

        private ReactionState GetReactionState(int reactionState)
        {
            try
            {
                return (ReactionState)Enum.ToObject(typeof(ReactionState), reactionState);
            }
            catch (Exception)
            {
                throw new ApiException(this.apiResultService.BadRequestResult((
                    ErrorCode.Parse(ErrorCodeType.PropertyInvalidData, AtanetEntityName.Reaction, PropertyName.Reaction.ReactionState),
                    new ErrorDefinition(reactionState, "Invalid reaction state", PropertyName.Reaction.ReactionState)
                )));
            }
        }
    }
}
