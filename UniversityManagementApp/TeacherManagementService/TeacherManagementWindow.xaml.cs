using System.Windows;
using UniversityManagement.Data;

namespace UniversityManagement.TeacherManagementService;

public partial class TeacherManagementWindow
{
    private UniversityDbContext _dbContext;
    private TeacherService _teacherService;

    public TeacherManagementWindow(UniversityDbContext dbContext, TeacherService teacherService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _teacherService = teacherService;
    }

    private void EditTeacher_Click(object sender, RoutedEventArgs e)
    {
        var editTeacherPage = new TeacherManagementWindowEditTeacherPage(_dbContext, _teacherService);

        EditTeacherButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ChangeTeacherDataButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editTeacherPage;
    }

    private void ChangeTeacherData_Click(object sender, RoutedEventArgs e)
    {
        var changeTeacherDataPage = new TeacherManagementWindowChangeTeacherDataPage(_dbContext, _teacherService);

        EditTeacherButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeTeacherDataButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = changeTeacherDataPage;
    }
}