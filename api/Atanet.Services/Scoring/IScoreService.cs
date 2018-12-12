namespace Atanet.Services.Scoring
{
    using System.Linq;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public enum AtanetAction
    {
        DeleteLowScoreUser,
        CreateComment,
        ViewUserProfile,
        CreatePost,
        VotePost,
        ViewOwnUserProfile
    }

    public interface IScoreService
    {
        double CalculateUserScore(long userId);

        IQueryable<PostWithScoreDto> GetEnrichedPosts();

        bool Can(AtanetAction action, long userId);

        long GetMinScore(AtanetAction action);
    }
}
