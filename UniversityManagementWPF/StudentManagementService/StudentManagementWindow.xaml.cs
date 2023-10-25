using System.Windows;
using UniversityManagement.DataAccess;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.StudentManagementService;

public partial class StudentManagementWindow
{
    private UniversityDbContext _dbContext;
    private StudentService _studentService;
    private CsvService _csvService;

    public StudentManagementWindow(UniversityDbContext dbContext, StudentService studentService, CsvService csvService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _studentService = studentService;
        _csvService = csvService;
    }

    private void ManageStudentsGroupButton_Click(object sender, RoutedEventArgs e)
    {
        var manageStudentGroupPage = new StudentManagementWindowManageStudentsGroupPage(_dbContext, _studentService);
    
        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");
    
        MainFrame.Content = manageStudentGroupPage;
    }

    private void EditStudentsButton_Click(object sender, RoutedEventArgs e)
    {
        var editStudentPage = new StudentManagementWindowEditStudentPage(_dbContext, _studentService);

        EditStudentsButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editStudentPage;
    }

    private void ChangeStudentData_Click(object sender, RoutedEventArgs e)
    {
        var changeStudentDataPage = new StudentManagementWindowChangeStudentDataPage(_dbContext, _studentService);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("HighlightedButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = changeStudentDataPage;
    }

    private void ExportOrImportStudentToGroup_Click(object sender, RoutedEventArgs e)
    {
        var exportOrImportStudentsToGroup = new StudentManagementWindowExpOrImpStudentsToGroupPage(_dbContext, _csvService);

        EditStudentsButton.Style = (Style)FindResource("NormalButtonStyle");
        ManageStudentsGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        ChangeStudentDataButton.Style = (Style)FindResource("NormalButtonStyle");
        ExpOrImpStudentsToGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = exportOrImportStudentsToGroup;
    }
}