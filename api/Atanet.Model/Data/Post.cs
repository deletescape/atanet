namespace Atanet.Model.Data
{
    using Atanet.Model.Interfaces;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Post : IIdentifiable, ICreated, IUserCreatedEntity
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar(10000) character set utf8mb4")]
        public string Text { get; set; }

        public long PictureId { get; set; }

        public File Picture { get; set; }

        public DateTime Created { get; set; }

        public float Sentiment { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }
    }
}
