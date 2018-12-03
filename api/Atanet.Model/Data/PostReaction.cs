namespace Atanet.Model.Data
{
    using Atanet.Model.Interfaces;

    public class PostReaction : IIdentifiable, IUserCreatedEntity
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public long PostId { get; set; }

        public Post Post { get; set; }

        public ReactionState ReactionState { get; set; }
    }
}