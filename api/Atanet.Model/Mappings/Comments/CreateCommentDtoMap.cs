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
                .ForMember(x => x.Text, x => x.MapFrom(p => p.Text))
                .ForMember(x => x.UserId, x => x.Ignore())
                .ForMember(x => x.User, x => x.Ignore())
                .ForMember(x => x.PostId, x => x.Ignore())
                .ForMember(x => x.Post, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Created, x => x.Ignore());
        }
    }
}
