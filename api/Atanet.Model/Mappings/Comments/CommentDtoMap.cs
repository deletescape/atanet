﻿namespace Atanet.Model.Mappings.Comments
{
    using AutoMapper;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public class CommentDtoMap : Profile
    {
        public CommentDtoMap()
        {
            this.CreateMap<Comment, CommentDto>()
                .ForMember(x => x.Created, x => x.MapFrom(p => p.Created))
                .ForMember(x => x.Id, x => x.MapFrom(p => p.Id))
                .ForMember(x => x.Text, x => x.MapFrom(p => p.Text))
                .ForMember(x => x.PostId, x => x.MapFrom(p => p.PostId))
                .ForMember(x => x.User, x => x.Ignore())
                .ReverseMap();
        }
    }
}
