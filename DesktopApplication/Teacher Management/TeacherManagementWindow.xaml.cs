using System.Windows;

namespace DesktopApplication.Teacher_Management;

public partial class TeacherManagementWindow
{
    private DataRepository _dataRepository;

    public TeacherManagementWindow(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
    }

    private void EditTeacher_Click(object sender, RoutedEventArgs e)
    {
        var editTeacherPage = new TeacherManagementWindowEditTeacherPage(_dataRepository);

        EditTeacherButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ChangeTeacherDataButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editTeacherPage;
    }

    private void ChangeTeacherData_Click(object sender, RoutedEventArgs e)
    {
        var changeTeacherDataPage = new TeacherManagementWindowChangeTeacherDataPage(_dataRepository);

        EditTeacherButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeTeacherDataButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = changeTeacherDataPage;
    }
}