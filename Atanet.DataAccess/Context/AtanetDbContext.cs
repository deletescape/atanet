namespace Atanet.DataAccess.Context
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Atanet.Model.Data;

    public class AtanetDbContext : IdentityDbContext
    {
        public AtanetDbContext(DbContextOptions<AtanetDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<LocationName> LocationNames { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
