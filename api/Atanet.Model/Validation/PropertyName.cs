namespace Atanet.Model.Validation
{
    using System;

    public class PropertyName : IEquatable<PropertyName>
    {
        public static readonly PropertyName Empty = new PropertyName("NONE");

        internal PropertyName(string propertyName) =>
            this.Name = propertyName;

        public string Name { get; }
        
        public static implicit operator string(PropertyName name) =>
            name.Name;

        public static explicit operator PropertyName(string name) =>
            new PropertyName(name);

        public static bool operator ==(PropertyName left, PropertyName right) =>
            left.Name == right.Name;

        public static bool operator !=(PropertyName left, PropertyName right) =>
            left.Name != right.Name;

        public static PropertyName Parse(string name) =>
            new PropertyName(name);

        public bool Equals(PropertyName other) =>
            this.Name == other.Name;

        public override int GetHashCode() =>
            this.Name.GetHashCode();

        public override bool Equals(object obj) =>
            obj is PropertyName propertyName && propertyName.Name == this.Name;

        public static class Post
        {
            public static readonly PropertyName Id = new PropertyName("ID");

            public static readonly PropertyName Text = new PropertyName("TEXT");

            public static readonly PropertyName Query = new PropertyName("QUERY");
        }

        public static class Filter
        {
            public static readonly PropertyName PageSize = new PropertyName("PAGE_SIZE");

            public static readonly PropertyName PageNumber = new PropertyName("PAGE");

            public static readonly PropertyName CommentAmount = new PropertyName("COMMENTS");
        }

        public static class Comment
        {
            public static readonly PropertyName PostId = new PropertyName("POST_ID");
        }

        public static class File
        {
            public static readonly PropertyName Id = new PropertyName("FILE_ID");

            public static readonly PropertyName ContentType = new PropertyName("CONTENT_TYPE");

            public static readonly PropertyName Data = new PropertyName("DATA");
        }

        public static class Reaction
        {
            public static readonly PropertyName Id = new PropertyName("REACTION_ID");

            public static readonly PropertyName ReactionState = new PropertyName("REACTION_STATE");
        }
    }
}
