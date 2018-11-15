namespace Atanet.Model.Mappings.Posts
{
    using AutoMapper;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public class CreatePostDtoMap : Profile
    {
        public CreatePostDtoMap()
        {
            this.CreateMap<CreatePostDto, Post>()
                .ForMember(x => x.Created, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Text, x => x.MapFrom(p => p.Text))
                .ForMember(x => x.Topic, x => x.Ignore())
                .ForMember(x => x.Longitude, x => x.MapFrom(p => p.Longitude))
                .ForMember(x => x.Latitude, x => x.MapFrom(p => p.Latitude))
                .ForMember(x => x.FileId, x => x.MapFrom(p => p.FileId))
                .ForMember(x => x.File, x => x.Ignore())
                .ForMember(x => x.LocationNameId, x => x.Ignore())
                .ForMember(x => x.LocationName, x => x.Ignore());
        }
    }
}
