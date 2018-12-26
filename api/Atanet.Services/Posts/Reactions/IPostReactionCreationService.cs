namespace Atanet.Services.Posts.Reactions
{
    using System.Collections.Generic;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public interface IPostReactionCreationService
    {
        IDictionary<ReactionState, int> AddReaction(long postId, CreateReactionDto createReactionDto);
    }
}
