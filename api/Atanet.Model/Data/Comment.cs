namespace Atanet.Model.Data
{
    using Atanet.Model.Interfaces;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Comment : IIdentifiable, ICreated, IUserCreatedEntity
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar(10000) character set utf8mb4")]
        public string Text { get; set; }

        public long PostId { get; set; }

        public Post Post { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public DateTime Created { get; set; }
    }
}
