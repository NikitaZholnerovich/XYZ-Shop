using Microsoft.EntityFrameworkCore;
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Infrastructure.Data
{
    public class XyzDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<PublisherEntity> Publishers { get; set; }
        public DbSet<UserProfileEntity> UserProfiles { get; set; }
        public DbSet<GameGenreEntity> GameGenres { get; set; }
        public DbSet<GameReviewEntity> GameReviews { get; set; }
        public DbSet<CommunityChatMessageEntity> CommunityChatMessages { get; set; }

        public XyzDbContext(DbContextOptions<XyzDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameEntity>()
              .HasOne(x => x.Publisher)
              .WithMany(x => x.Games)
              .HasForeignKey(x => x.PublisherId);

            modelBuilder.Entity<UserEntity>()
                .HasMany(x => x.MyFriends)
                .WithMany(x => x.WhoIsMyFriends);

            modelBuilder.Entity<GameEntity>()
               .HasOne(x => x.CreatedByUser)
               .WithMany(x => x.CreatedGames)
               .HasForeignKey(x => x.CreatedByUserId);

            modelBuilder.Entity<GameEntity>()
               .HasOne(x => x.ModifiedByUser)
               .WithMany(x => x.ModifiedGames)
               .HasForeignKey(x => x.ModifiedByUserId);

            modelBuilder.Entity<GameReviewEntity>()
                .HasOne(x => x.Author)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameReviewEntity>()
                .HasOne(x => x.Game)
                .WithMany(x => x.GameReviews)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEntity>()
                .HasOne(x => x.UserProfile)
                .WithOne(x => x.User)
                .HasForeignKey<UserEntity>(x => x.UserProfileId);

            modelBuilder.Entity<CommunityChatMessageEntity>()
                .HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CommunityChatMessages)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEntity>()
                .HasOne(x => x.UserProfile)
                .WithOne(x => x.User)
                .HasForeignKey<UserEntity>(x => x.UserProfileId);

            modelBuilder.Entity<GameEntity>()
               .HasMany(e => e.GameGenres)
               .WithMany(e => e.Games)
               .UsingEntity("GamesToGenres");

            base.OnModelCreating(modelBuilder);
        }
    }
}
