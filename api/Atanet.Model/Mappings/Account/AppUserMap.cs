namespace Atanet.Model.Mappings.Account
{
    using AutoMapper;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public class AppUserMap : Profile
    {
        public AppUserMap()
        {
            this.CreateMap<RegisterDto, AppUser>()
                .ForMember(x => x.UserName, x => x.MapFrom(p => p.Email))
                .ForMember(x => x.FirstName, x => x.MapFrom(p => p.FirstName))
                .ForMember(x => x.LastName, x => x.MapFrom(p => p.LastName));
        }
    }
}
