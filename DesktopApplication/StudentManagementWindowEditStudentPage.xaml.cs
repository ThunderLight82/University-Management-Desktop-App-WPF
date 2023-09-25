using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class StudentManagementWindowEditStudentPage : Page
{
    private DataRepository _dataRepository;

    public StudentManagementWindowEditStudentPage(DataRepository dataRepository) 
    { 
        InitializeComponent();
        _dataRepository = dataRepository;
        StudentsListView.ItemsSource = _dataRepository.Students;
    }

    private void AddStudent_Click(object sender, RoutedEventArgs e)
    {
        string newStudentFullName = NewStudentFullNameTextBox.Text.Trim();

        if (!string.IsNullOrWhiteSpace(newStudentFullName))
        {
            bool newStudentNameAlreadyExists = _dataRepository.Students.Any(student => 
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

            int lastNewestStudentId = _dataRepository.Students.Max(student => student.StudentId);

            var newStudent = new Student
            {
                StudentId = lastNewestStudentId + 1,
                StudentFullName = newStudentFullName,
                IsWorkingInDepartment = false
            };

            _dataRepository.Students.Add(newStudent);

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
        Student selectedStudent = StudentsListView.SelectedItem as Student;

        if (selectedStudent != null)
        {
            Group associatedGroup = _dataRepository.Groups.FirstOrDefault(group => group.Students.Contains(selectedStudent));

            if (associatedGroup != null)
            {
                associatedGroup.Students.Remove(selectedStudent);
            }

            selectedStudent.CurrentGroupName = null;

            _dataRepository.Students.Remove(selectedStudent);

            StudentsListView.Items.Refresh();
        }
        else
        {
            MessageBox.Show("Please, select a student from the list below to remove", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}