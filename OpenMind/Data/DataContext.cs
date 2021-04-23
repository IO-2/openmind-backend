using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenMind.Domain;
using OpenMind.Models;

namespace OpenMind.Data
{
    public class DataContext : IdentityDbContext<UserModel>
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<CoursePageModel> CoursesPages { get; set; }
        public DbSet<MediaModel> Media { get; set; }
        public DbSet<UserProgressBySectionModel> UsersProgressBySection { get; set; }
        public DbSet<UserRateCourseModel> UserRateCourses { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<InterestModel>()
                .HasOne(p => p.User)
                .WithMany(t => t.Interests)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshTokenModel>()
                .HasOne(p => p.User)
                .WithOne(t => t.RefreshToken)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserProgressBySectionModel>()
                .HasOne(p => p.User)
                .WithMany(t => t.Progresses)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<UserRateCourseModel>()
                .HasOne(p => p.User)
                .WithMany(t => t.UserRatresCources)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}