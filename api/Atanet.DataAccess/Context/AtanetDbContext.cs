namespace Atanet.DataAccess.Context
{
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PostReaction>()
                .HasAlternateKey(x => new { x.PostId, x.UserId });
            modelBuilder.Entity<User>()
                .HasAlternateKey(x => x.Email);
                
            modelBuilder.Entity<User>().HasMany<Post>().WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany<PostReaction>().WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasOne(x => x.Picture).WithOne().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>().HasMany<PostReaction>().WithOne(x => x.Post).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Post>().HasOne(x => x.Picture).WithMany().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
