namespace Atanet.Services.Comments
{
    using System.Linq;
    using Atanet.Model.ApiResponse.HTTP400;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Extensions;
    using Atanet.Model.Validation;
    using Atanet.Services.Exceptions;
    using Atanet.Services.UoW;

    public class CommentCreationService : ICommentCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IQueryService queryService;

        public CommentCreationService(IUnitOfWorkFactory unitOfWorkFactory, IQueryService queryService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.queryService = queryService;
        }

        public long CreateComment(long postId, CreateCommentDto createCommentDto)
        {
            if (!this.queryService.Query<Post>().Any(x => x.Id == postId))
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, AtanetEntityName.Comment, PropertyName.Comment.PostId, AtanetEntityName.Post),
                    new ErrorDefinition(postId, "The given post does not exist", PropertyName.Comment.PostId))));
            }

            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var comment = createCommentDto.MapTo<Comment>();
                comment.PostId = postId;
                var commentRepository = unitOfWork.CreateEntityRepository<Comment>();
                commentRepository.Create(comment);
                unitOfWork.Save();
                return comment.Id;
            }
        }
    }
}
