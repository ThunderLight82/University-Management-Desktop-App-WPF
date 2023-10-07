using System.Collections.Generic;
using System.Windows;
using DesktopApplication.Entities;

namespace DesktopApplication.StudentManagementService;

public partial class StudentManagementWindow
{
    private UniversityDbContext _dbContext;
    private HashSet<Student> _assignedStudents;

    public StudentManagementWindow(UniversityDbContext dbContext, HashSet<Student> assignedStudents)
    {
        InitializeComponent();
        _assignedStudents = assignedStudents;
        _dbContext = dbContext;
    }

    private void ManageStudentsGroupButton_Click(object sender, RoutedEventArgs e)
    {
        var manageStudentGroupPage = new StudentManagementWindowManageStudentsGroupPage(_dbContext, _assignedStudents);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = manageStudentGroupPage;
    }

    private void EditStudentsButton_Click(object sender, RoutedEventArgs e)
    {
        var editStudentPage = new StudentManagementWindowEditStudentPage(_dbContext);

        EditStudentsButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editStudentPage;
    }

    private void ChangeStudentData_Click(object sender, RoutedEventArgs e)
    {
        var changeStudentDataPage = new StudentManagementWindowChangeStudentDataPage(_dbContext);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = changeStudentDataPage;
    }

    private void ExportOrImportStudentToGroup_Click(object sender, RoutedEventArgs e)
    {
        var exportOrImportStudentsToGroup = new StudentManagementWindowExpOrImpStudentsToGroupPage(_dbContext);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = exportOrImportStudentsToGroup;
    }
}