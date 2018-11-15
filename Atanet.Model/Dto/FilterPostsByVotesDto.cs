namespace Atanet.Model.Dto
{
    using Atanet.Model.Interfaces;

    public class FilterPostsByVotesDto : IPaged
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
