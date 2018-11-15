namespace Atanet.Model.Dto
{
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.Interfaces;

    public class PagedDto : IPaged
    {
        [FromQuery(Name = "page")]
        public int PageNumber { get; set; }

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }
    }
}
