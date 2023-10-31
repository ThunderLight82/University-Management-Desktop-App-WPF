using System.Windows;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.TeacherManagementService;

public partial class TeacherManagementWindow
{
    private TeacherService _teacherService;

    public TeacherManagementWindow(TeacherService teacherService)
    {
        InitializeComponent();

        _teacherService = teacherService;
    }

    private void EditTeacher_Click(object sender, RoutedEventArgs e)
    {
        var editTeacherPage = new TeacherManagementWindowEditTeacherPage(_teacherService);

        EditTeacherButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ChangeTeacherDataButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editTeacherPage;
    }

    private void ChangeTeacherData_Click(object sender, RoutedEventArgs e)
    {
        var changeTeacherDataPage = new TeacherManagementWindowChangeTeacherDataPage(_teacherService);

        EditTeacherButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeTeacherDataButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = changeTeacherDataPage;
    }
}