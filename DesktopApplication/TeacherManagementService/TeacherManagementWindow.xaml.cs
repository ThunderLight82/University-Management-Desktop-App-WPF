using System.Windows;

namespace DesktopApplication.TeacherManagementService;

public partial class TeacherManagementWindow
{
    private UniversityDbContext _dbContext;

    public TeacherManagementWindow(UniversityDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
    }

    private void EditTeacher_Click(object sender, RoutedEventArgs e)
    {
        var editTeacherPage = new TeacherManagementWindowEditTeacherPage(_dbContext);

        EditTeacherButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ChangeTeacherDataButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editTeacherPage;
    }

    private void ChangeTeacherData_Click(object sender, RoutedEventArgs e)
    {
        var changeTeacherDataPage = new TeacherManagementWindowChangeTeacherDataPage(_dbContext);

        EditTeacherButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeTeacherDataButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = changeTeacherDataPage;
    }
}