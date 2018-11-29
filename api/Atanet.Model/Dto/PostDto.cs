namespace Atanet.Model.Dto
{
    using System;

    public class PostDto
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public long VoteCount { get; set; }

        public DateTime Created { get; set; }

        public CommentDto[] Comments { get; set; }

        public FileInfoDto File { get; set; }
    }
}
