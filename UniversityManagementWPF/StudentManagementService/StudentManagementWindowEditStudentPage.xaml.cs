using System.Windows;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.StudentManagementService;

public partial class StudentManagementWindowEditStudentPage
{
    private StudentService _studentService;

    public StudentManagementWindowEditStudentPage(StudentService studentService) 
    {
        InitializeComponent();

        _studentService = studentService;

        StudentsListView.ItemsSource = _studentService.PopulateStudentList();
    }

    private void AddStudent_Click(object sender, RoutedEventArgs e)
    {
        string newStudentFullName = NewStudentFullNameTextBox.Text.Trim();

        if (!string.IsNullOrWhiteSpace(newStudentFullName))
        {
            var newStudentNameAlreadyExists = _studentService.CheckIfStudentExists(newStudentFullName);

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

            _studentService.AddStudent(newStudentFullName);

            NewStudentFullNameTextBox.Clear();

            StudentsListView.ItemsSource = _studentService.PopulateStudentList();
        }
        else
        {
            MessageBox.Show("Please, enter a valid student name", 
                "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DeleteStudent_Click(object sender, RoutedEventArgs e)
    {
        if (StudentsListView.SelectedItem is Student selectedStudent)
        {
            _studentService.DeleteStudent(selectedStudent);

            StudentsListView.ItemsSource = _studentService.PopulateStudentList();
        }
        else
        {
            MessageBox.Show("Please, select a student from the list below to remove", 
                "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}