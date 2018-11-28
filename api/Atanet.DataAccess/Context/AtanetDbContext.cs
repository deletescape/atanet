namespace Atanet.DataAccess.Context
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Atanet.Model.Data;

    public class AtanetDbContext : DbContext
    {
        public AtanetDbContext(DbContextOptions<AtanetDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<PostReaction> PostReactions { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<File> Files { get; set; }
    }
}
