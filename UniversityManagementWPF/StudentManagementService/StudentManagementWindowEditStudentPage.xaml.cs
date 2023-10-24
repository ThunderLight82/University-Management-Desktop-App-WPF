using System.Windows;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.StudentManagementService;

public partial class StudentManagementWindowEditStudentPage
{
    private UniversityDbContext _dbContext;
    private StudentService _studentService;

    public StudentManagementWindowEditStudentPage(UniversityDbContext dbContext, StudentService studentService) 
    {
        InitializeComponent();

        _dbContext = dbContext;
        _studentService = studentService;

        StudentsListView.ItemsSource = _dbContext.Students.Local.ToObservableCollection();
    }

    private void AddStudent_Click(object sender, RoutedEventArgs e)
    {
        string newStudentFullName = NewStudentFullNameTextBox.Text.Trim();

        if (_studentService.AddStudent(newStudentFullName))
        {
            NewStudentFullNameTextBox.Clear();

            StudentsListView.Items.Refresh();
        }
    }

    private void DeleteStudent_Click(object sender, RoutedEventArgs e)
    {
        var selectedStudent = StudentsListView.SelectedItem as Student;

        if (_studentService.DeleteStudent(selectedStudent))
        {
            StudentsListView.Items.Refresh();
        }
    }
}