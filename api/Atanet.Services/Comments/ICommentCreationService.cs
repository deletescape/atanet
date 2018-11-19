namespace Atanet.Services.Comments
{
    using Atanet.Model.Dto;

    public interface ICommentCreationService
    {
        long CreateComment(long postId, CreateCommentDto createPostDto);
    }
}
