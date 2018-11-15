namespace Atanet.Services.Posts
{
    public interface IVoteCountService
    {
        long CountVotes(long postId);
    }
}
