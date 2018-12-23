namespace Atanet.Services.Posts.Sentiment
{
    public interface ISentimentService
    {
        float GetSentiment(string sentence);
    }
}