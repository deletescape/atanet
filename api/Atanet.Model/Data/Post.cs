namespace Atanet.Model.Data
{
    using Atanet.Model.Interfaces;
    using System;

    public class Post : IIdentifiable, ICreated, IUserCreatedEntity
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public long PictureId { get; set; }

        public File Picture { get; set; }

        public DateTime Created { get; set; }

        public float Sentiment { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }
    }
}
