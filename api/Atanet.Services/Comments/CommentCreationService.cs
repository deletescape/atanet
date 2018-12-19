namespace Atanet.Services.Comments
{
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
    using AutoMapper;

    public class CommentCreationService : ICommentCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IQueryService queryService;

        private readonly IUserService userService;

        private readonly IApiResultService apiResultService;

        private readonly IMapper mapper;

        private readonly IScoreService scoreService;

        public CommentCreationService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IQueryService queryService,
            IUserService userService,
            IApiResultService apiResultService,
            IMapper mapper,
            IScoreService scoreService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.queryService = queryService;
            this.userService = userService;
            this.apiResultService = apiResultService;
            this.mapper = mapper;
            this.scoreService = scoreService;
        }

        public long CreateComment(long postId, CreateCommentDto createCommentDto)
        {
            if (!this.queryService.Query<Post>().Any(x => x.Id == postId))
            {
                throw new ApiException(this.apiResultService.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, AtanetEntityName.Comment, PropertyName.Comment.PostId, AtanetEntityName.Post),
                    new ErrorDefinition(postId, "The given post does not exist", PropertyName.Comment.PostId))));
            }

            var currentUserId = this.userService.GetCurrentUserId();
            if (!this.scoreService.Can(AtanetAction.CreateComment, currentUserId))
            {
                var minScore = this.scoreService.GetMinScore(AtanetAction.CreateComment);
                throw new ApiException(this.apiResultService.BadRequestResult($"User must have a score greater than {minScore} in order to create comments"));
            }

            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var comment = this.mapper.Map<Comment>(createCommentDto);
                comment.PostId = postId;
                comment.UserId = currentUserId;
                var commentRepository = unitOfWork.CreateEntityRepository<Comment>();
                commentRepository.Create(comment);
                unitOfWork.Save();
                return comment.Id;
            }
        }
    }
}
