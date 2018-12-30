namespace Atanet.Model.Dto
{
    using System;
    using System.Collections.Generic;
    using Atanet.Model.Data;

    public class PostDto
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public double Score { get; set; }

        public DateTime Created { get; set; }

        public UserDto User { get; set; }

        public CommentDto[] Comments { get; set; }

        public IDictionary<ReactionState, int> Reactions { get; set; }
    }
}
