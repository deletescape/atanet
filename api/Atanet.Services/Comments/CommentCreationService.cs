namespace Atanet.Services.Comments
{
    using Atanet.Model.Data;
    using Atanet.Model.Dto;
    using Atanet.Model.Extensions;
    using Atanet.Services.UoW;

    public class CommentCreationService : ICommentCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public CommentCreationService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public long CreateComment(long postId, CreateCommentDto createCommentDto)
        {
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
