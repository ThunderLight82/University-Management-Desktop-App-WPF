using System.Windows;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.StudentManagementService;

public partial class StudentManagementWindow
{
    private StudentService _studentService;
    private CsvService _csvService;

    public StudentManagementWindow(StudentService studentService, CsvService csvService)
    {
        InitializeComponent();

        _studentService = studentService;
        _csvService = csvService;
    }

    private void ManageStudentsGroupButton_Click(object sender, RoutedEventArgs e)
    {
        var manageStudentGroupPage = new StudentManagementWindowManageStudentsGroupPage(_studentService);
    
        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");
    
        MainFrame.Content = manageStudentGroupPage;
    }

    private void EditStudentsButton_Click(object sender, RoutedEventArgs e)
    {
        var editStudentPage = new StudentManagementWindowEditStudentPage(_studentService);

        EditStudentsButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editStudentPage;
    }

    private void ChangeStudentData_Click(object sender, RoutedEventArgs e)
    {
        var changeStudentDataPage = new StudentManagementWindowChangeStudentDataPage(_studentService);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = changeStudentDataPage;
    }

    private void ExportOrImportStudentToGroup_Click(object sender, RoutedEventArgs e)
    {
        var exportOrImportStudentsToGroup = new StudentManagementWindowExpOrImpStudentsToGroupPage(_csvService);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = exportOrImportStudentsToGroup;
    }
}