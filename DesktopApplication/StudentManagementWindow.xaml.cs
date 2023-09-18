using System.Windows;
using System.Collections.Generic;

namespace DesktopApplication;

public partial class StudentManagementWindow : Window
{
    private DataRepository _dataRepository;
    private HashSet<Student> _assignedStudents;

    public StudentManagementWindow(DataRepository dataRepository, HashSet<Student> assignedStudents)
    {
        _dataRepository = dataRepository;
        _assignedStudents = assignedStudents;
        InitializeComponent();
    }

    private void ManageStudentsGroupButton_Click(object sender, RoutedEventArgs e)
    {
        var manageStudentGroupPage = new StudentManagementWindowManageStudentsGroupPage(_dataRepository, _assignedStudents);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = manageStudentGroupPage;
    }

    private void EditStudentsButton_Click(object sender, RoutedEventArgs e)
    {
        var editStudentPage = new StudentManagementWindowEditStudentPage(_dataRepository);

        EditStudentsButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editStudentPage;
    }

    private void ChangeStudentData_Click(object sender, RoutedEventArgs e)
    {
        var changeStudentData = new StudentManagementWindowChangeStudentData(_dataRepository);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = changeStudentData;
    }
}