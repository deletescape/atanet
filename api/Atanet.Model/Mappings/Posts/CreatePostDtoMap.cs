﻿namespace Atanet.Model.Mappings.Posts
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
                .ForMember(x => x.PictureId, x => x.Ignore())
                .ForMember(x => x.Picture, x => x.Ignore())
                .ForMember(x => x.UserId, x => x.Ignore())
                .ForMember(x => x.User, x => x.Ignore())
                .ForMember(x => x.Sentiment, x => x.Ignore());
        }
    }
}
