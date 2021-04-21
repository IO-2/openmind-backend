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
        public DbSet<ChecklistModel> Checklists { get; set; }
        public DbSet<LongreadModel> Longreads { get; set; }
        public DbSet<UserProgressBySectionModel> UsersProgressBySection { get; set; }
        
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}