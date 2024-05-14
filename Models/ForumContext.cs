using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
namespace portrait_forum.Models
{
    public class ForumContext : DbContext
    {
        public ForumContext(DbContextOptions<ForumContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().Property(p => p.DateCreated).HasDefaultValueSql("datetime()");
            modelBuilder.Entity<Conversation>().Property(p => p.DateCreated).HasDefaultValueSql("datetime()");
            modelBuilder.Entity<Board>().Property(p => p.DateCreated).HasDefaultValueSql("datetime()");
            modelBuilder.Entity<User>().Property(p => p.DateCreated).HasDefaultValueSql("datetime()");
            modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Board>()
                .HasMany(e => e.Conversations)
                .WithOne(e => e.Board)
                .HasForeignKey(e => e.BoardId)
                .HasPrincipalKey(e => e.Id);
            modelBuilder.Entity<Conversation>()
                .HasMany(e => e.Posts)
                .WithOne(e => e.Conversation)
                .HasForeignKey(e => e.ConversationID)
                .HasPrincipalKey(e => e.Id);
            modelBuilder.Entity<User>()
                .HasMany(e => e.Boards)
                .WithOne(e => e.Owner)
                .HasForeignKey(e => e.OwnerID)
                .HasPrincipalKey(e => e.Id);
            modelBuilder.Entity<User>()
                .HasMany(e => e.Conversations)
                .WithOne(e => e.Owner)
                .HasForeignKey(e => e.OwnerID)
                .HasPrincipalKey(e => e.Id);
            modelBuilder.Entity<User>()
                .HasMany(e => e.Posts)
                .WithOne(e => e.Owner)
                .HasForeignKey(e => e.OwnerID)
                .HasPrincipalKey(e => e.Id);
        }

        public DbSet<Board> Boards { get; set; } = null!;
        public DbSet<Conversation> Conversation { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}
