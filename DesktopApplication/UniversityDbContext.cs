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

    }
    public UniversityDbContext()
    {

    }
}