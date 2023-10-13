using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UniversityManagement.Data;

namespace UniversityManagement;

public partial class App
{
    private UniversityDbContext _dbContext;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // We use server connection string from "appsettings.json" file within "bin" folder.

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionBuilder = new DbContextOptionsBuilder<UniversityDbContext>();
        optionBuilder.UseSqlServer(configuration.GetConnectionString("UniversityDbContextConnection"));

        _dbContext = new UniversityDbContext(optionBuilder.Options);

        var mainWindow = new MainWindow(_dbContext);

        mainWindow.Show();
    }
}