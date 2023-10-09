using System.Windows;
using System.Windows.Controls;
using DesktopApplication.Entities;

namespace DesktopApplication.StudentManagementService;

public partial class StudentManagementWindowChangeStudentDataPage
{
    private UniversityDbContext _dbContext;
    private StudentService _studentService;

    public StudentManagementWindowChangeStudentDataPage(UniversityDbContext dbContext, StudentService studentService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _studentService = studentService;

        StudentsListView.ItemsSource = _dbContext.Students.Local.ToObservableCollection();
    }

    private void ChangeStudentNameAndWorkInfo_Click (object sender, RoutedEventArgs e)
    {
        var selectedStudent = StudentsListView.SelectedItem as Student;

        string changedStudentFullName = ChangeStudentFullNameTextBox.Text.Trim();

        if (IsWorkingComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            bool isWorkingInDepartment = selectedItem.Content.ToString() == "Yes";

            if (_studentService.ChangeStudentNameAndWorkInfo(selectedStudent, changedStudentFullName, isWorkingInDepartment))
            {
                StudentsListView.Items.Refresh();
            }
        }
    }

    private void FillInfoBlockWithSelectedStudentFromListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (StudentsListView.SelectedItem is Student selectedStudent)
        {
            ChangeStudentFullNameTextBox.Text = selectedStudent.StudentFullName;

            IsWorkingComboBox.SelectedIndex = selectedStudent.IsWorkingInDepartment ? 0 : 1;
        }
    }
}