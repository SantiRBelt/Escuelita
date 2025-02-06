using Microsoft.EntityFrameworkCore;
using Entregable_Universities.Models;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<CategoryModel> Categories { get; set; }
    public DbSet<CourseModel> Courses { get; set; }
    public DbSet<StudentModel> Students { get; set; }
    public DbSet<ChapterModel> Chapters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryModel>().ToTable("Category");
        modelBuilder.Entity<CourseModel>().ToTable("Course");
        modelBuilder.Entity<StudentModel>().ToTable("Student");
        modelBuilder.Entity<ChapterModel>().ToTable("Chapter");
        modelBuilder.Entity<UserModel>().ToTable("User");
        //modelBuilder.Entity<UserLogins>().ToTable("UserLogins");

    }
}

