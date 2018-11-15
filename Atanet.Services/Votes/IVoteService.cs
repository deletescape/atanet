namespace Atanet.Services.Votes
{
    using Atanet.Model.Data;

    public interface IVoteService
    {
        long Vote(long postId, VoteState voteState);
    }
}
