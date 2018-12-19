namespace Atanet.Services.Posts.Reactions
{
    using System;
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

        public long AddReaction(long postId, CreateReactionDto createReactionDto)
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

            if (this.queryService.Query<PostReaction>().Any(x => x.PostId == postId && x.UserId == currentUserId))
            {
                throw new ApiException(this.apiResultService.BadRequestResult((
                    ErrorCode.Parse(ErrorCodeType.PropertyInvalidData, AtanetEntityName.Reaction, PropertyName.Reaction.Id),
                    new ErrorDefinition(createReactionDto, "User has already voted on this post", PropertyName.Reaction.Id))));
            }

            var state = this.GetReactionState(createReactionDto.ReactionState.Value);
            var postReaction = new PostReaction
            {
                PostId = postId,
                ReactionState = state,
                UserId = currentUserId
            };
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var postReactionRepository = unitOfWork.CreateEntityRepository<PostReaction>();
                postReactionRepository.Create(postReaction);
                unitOfWork.Save();
            }

            return postReaction.Id;
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
