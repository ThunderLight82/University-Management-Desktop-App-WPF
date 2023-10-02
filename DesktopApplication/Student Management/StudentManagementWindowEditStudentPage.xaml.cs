using System;
using System.Linq;
using System.Windows;

namespace DesktopApplication.Student_Management;

public partial class StudentManagementWindowEditStudentPage
{
    private UniversityDbContext _dbContext;

    public StudentManagementWindowEditStudentPage(UniversityDbContext dbContext) 
    {
        InitializeComponent();
        _dbContext = dbContext;
        StudentsListView.ItemsSource = _dbContext.Students;
        //Maybe use "StudentsListView.ItemsSource = _dbContext.Students.ToList()" instead???
    }

    private void AddStudent_Click(object sender, RoutedEventArgs e)
    {
        string newStudentFullName = NewStudentFullNameTextBox.Text.Trim();

        if (!string.IsNullOrWhiteSpace(newStudentFullName))
        {
            var newStudentNameAlreadyExists = _dbContext.Students.Any(student => 
                student.StudentFullName.Equals(newStudentFullName, StringComparison.OrdinalIgnoreCase));

            if (newStudentNameAlreadyExists)
            {
                var duplicateStudentQuestion = MessageBox.Show(
                    "A student with the same name already exists. Do you want to add this student anyway?",
                    "Duplication name",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (duplicateStudentQuestion == MessageBoxResult.No)
                {
                    return;
                }
            }

            int lastNewestStudentId = _dbContext.Students.Max(student => student.StudentId);

            var newStudent = new Student
            {
                StudentId = lastNewestStudentId + 1,
                StudentFullName = newStudentFullName,
                IsWorkingInDepartment = false
            };

            _dbContext.Students.Add(newStudent);
            _dbContext.SaveChanges();

            NewStudentFullNameTextBox.Clear();

            StudentsListView.Items.Refresh();
        }
        else
        {
            MessageBox.Show("Please, enter a valid student name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DeleteStudent_Click(object sender, RoutedEventArgs e)
    {
        var selectedStudent = StudentsListView.SelectedItem as Student;

        if (selectedStudent != null)
        {
            var associatedGroup = _dbContext.Groups.FirstOrDefault(group => group.Students.Contains(selectedStudent));

            associatedGroup?.Students.Remove(selectedStudent);

            selectedStudent.CurrentGroupName = null;

            _dbContext.Students.Remove(selectedStudent);
            _dbContext.SaveChanges();

            StudentsListView.Items.Refresh();
        }
        else
        {
            MessageBox.Show("Please, select a student from the list below to remove", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}