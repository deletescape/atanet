namespace Atanet.Services.Comments
{
    using Atanet.Model.Dto;
    using System;
    using System.Collections.Generic;

    public interface ICommentFilterService
    {
        IEnumerable<CommentDto> GetCommentsForPost(long postId, int page, int pageSize);
    }
}
