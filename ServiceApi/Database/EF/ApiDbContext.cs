using Microsoft.EntityFrameworkCore;
using ServiceApi.Models;

namespace ServiceApi.Database.EF;

public class ApiDbContext
{
    public partial class ThisDbContext : DbContext
    {
        public ThisDbContext() { }
        public ThisDbContext(DbContextOptions<ThisDbContext> options) : base(options) { }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Models.Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<UserSkill> UserSkills { get; set; }
        public virtual DbSet<News> News { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSkill>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSkills)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSkills_User");

                entity.ToTable("UserSkills");
            });
            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Description)
                   .IsRequired()
                   .HasMaxLength(1500);
                entity.Property(e => e.Url)
                   .IsRequired()
                   .HasMaxLength(100);
                entity.Property(e => e.Ts)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("Ts");

                entity.ToTable("News");
            });
            modelBuilder.Entity<RefreshToken>(entity =>
            {

                entity.Property(e => e.ExpiryDate).HasColumnType("smalldatetime");

                entity.Property(e => e.TokenHash)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.TokenSalt)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Ts)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("TS");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefreshToken_User");

                entity.ToTable("RefreshToken");
            });

            modelBuilder.Entity<Models.Task>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Ts)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("TS");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_User");

                entity.HasOne(d => d.Category)
                   .WithMany(p => p.Tasks)
                   .HasForeignKey(d => d.CategoryId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Task_Category");

                entity.ToTable("Task");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Ts)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("TS");

                entity.ToTable("User");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.ToTable("Category");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
