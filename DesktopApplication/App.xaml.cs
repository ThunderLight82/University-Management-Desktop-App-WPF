using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace DesktopApplication;

public partial class App
{
    private UniversityDbContext _dbContext;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Hardcoded Server initialization instead of using it in appsettings.json(I add it later)

        var optionsBuilder = new DbContextOptionsBuilder<UniversityDbContext>();
        optionsBuilder.UseSqlServer("Data Source=THUNDERLIGHT;" +
                                    " Initial Catalog=UniversityServerDB;" +
                                    " Integrated Security=True;TrustServerCertificate=true;");

        _dbContext = new UniversityDbContext(optionsBuilder.Options);

        var mainWindow = new MainWindow();
        mainWindow.DataContext = _dbContext;

        mainWindow.Show();
    }
}