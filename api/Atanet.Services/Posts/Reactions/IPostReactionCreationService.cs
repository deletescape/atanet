namespace Atanet.Services.Posts.Reactions
{
    using Atanet.Model.Dto;

    public interface IPostReactionCreationService
    {
        long AddReaction(long postId, CreateReactionDto createReactionDto);
    }
}
