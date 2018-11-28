namespace Atanet.Model.Mappings.Posts
{
    using AutoMapper;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public class PostDtoMap : Profile
    {
        public PostDtoMap()
        {
            this.CreateMap<Post, PostDto>()
                .ForMember(x => x.Created, x => x.MapFrom(y => y.Created))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Text, x => x.MapFrom(y => y.Text))
                .ForMember(x => x.VoteCount, x => x.Ignore())
                .ForMember(x => x.Comments, x => x.Ignore())
                .ForMember(x => x.File, x => x.Ignore())
                .ReverseMap();
        }
    }
}
