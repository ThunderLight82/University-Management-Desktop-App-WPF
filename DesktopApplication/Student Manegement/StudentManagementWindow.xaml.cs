using System.Windows;
using System.Collections.Generic;

namespace DesktopApplication;

public partial class StudentManagementWindow : Window
{
    private DataRepository _dataRepository;
    private HashSet<Student> _assignedStudents;

    public StudentManagementWindow(DataRepository dataRepository, HashSet<Student> assignedStudents)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
        _assignedStudents = assignedStudents;
    }

    private void ManageStudentsGroupButton_Click(object sender, RoutedEventArgs e)
    {
        var manageStudentGroupPage = new StudentManagementWindowManageStudentsGroupPage(_dataRepository, _assignedStudents);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = manageStudentGroupPage;
    }

    private void EditStudentsButton_Click(object sender, RoutedEventArgs e)
    {
        var editStudentPage = new StudentManagementWindowEditStudentPage(_dataRepository);

        EditStudentsButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editStudentPage;
    }

    private void ChangeStudentData_Click(object sender, RoutedEventArgs e)
    {
        var changeStudentDataPage = new StudentManagementWindowChangeStudentDataPage(_dataRepository);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = changeStudentDataPage;
    }

    private void ExportOrImportStudentToGroup_Click(object sender, RoutedEventArgs e)
    {
        var exportOrImportStudentsToGroup = new StudentManagementWindowExpOrImpStudentsToGroupPage(_dataRepository);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = exportOrImportStudentsToGroup;
    }
}