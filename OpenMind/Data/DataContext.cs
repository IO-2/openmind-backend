using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenMind.Models.Courses;
using OpenMind.Models.Media;
using OpenMind.Models.Users;

namespace OpenMind.Data
{
    public class DataContext : IdentityDbContext<UserModel>
    {
        // Users
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserProgressBySectionModel> UsersProgressBySection { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        
        // Courses
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<CourseLessonModel> CoursesPages { get; set; }
        
        public DbSet<UserRateCourseModel> UserRateCourses { get; set; }
        
        // Media
        public DbSet<MediaModel> Media { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
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
            
            modelBuilder.Entity<UserRateCourseModel>()
                .HasOne(p => p.Course)
                .WithMany(t => t.Rates)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<CourseCardModel>()
                .HasOne(p => p.Course)
                .WithMany(t => t.Cards)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<CourseBenefitersModel>()
                .HasOne(p => p.Course)
                .WithMany(t => t.Benefiters)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<CourseLessonModel>()
                .HasOne(p => p.Course)
                .WithMany(t => t.Lessons)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}