namespace Atanet.Model.Data
{
    using Atanet.Model.Interfaces;
    using System;

    public class User : IIdentifiable, ICreated
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public DateTime Created { get; set; }

        public File Picture { get; set; }

        public long PictureId { get; set; }
    }
}
