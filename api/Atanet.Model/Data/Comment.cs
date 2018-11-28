namespace Atanet.Model.Data
{
    using Atanet.Model.Interfaces;
    using System;

    public class Comment : IIdentifiable, ICreated
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public long PostId { get; set; }

        public Post Post { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public DateTime Created { get; set; }
    }
}
