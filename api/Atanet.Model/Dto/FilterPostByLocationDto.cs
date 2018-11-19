namespace Atanet.Model.Dto
{
    using Microsoft.AspNetCore.Mvc;
    using Atanet.Model.Interfaces;

    public class FilterPostByLocationDto : PagedPostDto, ILocatable
    {
        [FromQuery(Name = "lng")]
        public double Longitude { get; set; }

        [FromQuery(Name = "lat")]
        public double Latitude { get; set; }
    }
}
