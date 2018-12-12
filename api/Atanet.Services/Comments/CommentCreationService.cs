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
    using Atanet.Services.UoW;
    using AutoMapper;

    public class CommentCreationService : ICommentCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IQueryService queryService;

        private readonly IUserService userService;

        private readonly IApiResultService apiResultService;

        private readonly IMapper mapper;

        public CommentCreationService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IQueryService queryService,
            IUserService userService,
            IApiResultService apiResultService,
            IMapper mapper)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.queryService = queryService;
            this.userService = userService;
            this.apiResultService = apiResultService;
            this.mapper = mapper;
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
