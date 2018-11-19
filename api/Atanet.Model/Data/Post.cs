namespace Atanet.Model.Data
{
    using Atanet.Model.Interfaces;
    using System;

    public class Post : IIdentifiable, ICreated, ILocatable
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public long? FileId { get; set; }

        public File File { get; set; }

        public DateTime Created { get; set; }
    }
}
