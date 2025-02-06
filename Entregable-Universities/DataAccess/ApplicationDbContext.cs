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
        modelBuilder.Entity<CategoryModel>().ToTable("category");
        modelBuilder.Entity<CourseModel>().ToTable("course");
        modelBuilder.Entity<StudentModel>().ToTable("student");
        modelBuilder.Entity<ChapterModel>().ToTable("chapter");
        modelBuilder.Entity<UserModel>().ToTable("user");
        //modelBuilder.Entity<UserLogins>().ToTable("UserLogins");

    }
}

