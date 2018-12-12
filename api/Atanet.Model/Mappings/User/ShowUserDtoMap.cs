namespace Atanet.Model.Mappings.User
{
    using AutoMapper;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public class ShowUserDtoMap : Profile
    {
        public ShowUserDtoMap()
        {
            this.CreateMap<User, ShowUserDto>()
                .ForMember(x => x.Created, x => x.MapFrom(p => p.Created))
                .ForMember(x => x.Id, x => x.MapFrom(p => p.Id))
                .ForMember(x => x.Email, x => x.MapFrom(p => p.Email))
                .ForMember(x => x.Capabilities, x => x.Ignore());
        }
    }
}
