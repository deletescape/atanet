namespace Atanet.Services.Posts
{
    using Atanet.Model.Dto;
    using System;
    using System.Collections.Generic;

    public interface IPostFilterService
    {
        IList<PostDto> FilterPosts(int page, int pageSize, int commentCount);
    }
}
