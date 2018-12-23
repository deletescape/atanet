namespace Atanet.Services.Scoring
{
    using System.Collections.Generic;
    using System.Linq;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public interface IScoreService
    {
        double CalculateUserScore(long userId);

        IQueryable<PostWithScoreDto> GetEnrichedPosts(bool withTimeInCalculation);

        bool Can(AtanetAction action, long userId);

        long GetMinScore(AtanetAction action);

        IEnumerable<AtanetAction> GetUserCapabilities(long userId);
    }
}
