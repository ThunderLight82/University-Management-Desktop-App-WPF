using Microsoft.EntityFrameworkCore;

namespace DesktopApplication;

public class UniversityDbContext : DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }

    public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
    {
        this.Courses.Load();
        this.Groups.Load();
        this.Teachers.Load();
        this.Students.Load();
    }
    public UniversityDbContext()
    {

    }
}