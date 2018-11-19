namespace Atanet.Model.Mappings.Comments
{
    using AutoMapper;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public class CreateCommentDtoMap : Profile
    {
        public CreateCommentDtoMap()
        {
            this.CreateMap<CreateCommentDto, Comment>()
                .ForMember(x => x.Latitude, x => x.MapFrom(p => p.Latitude))
                .ForMember(x => x.Longitude, x => x.MapFrom(p => p.Longitude))
                .ForMember(x => x.Text, x => x.MapFrom(p => p.Text));
        }
    }
}
