using System.Windows;
using System.Windows.Controls;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.StudentManagementService;

public partial class StudentManagementWindowChangeStudentDataPage
{
    private StudentService _studentService;

    public StudentManagementWindowChangeStudentDataPage(StudentService studentService)
    {
        InitializeComponent();

        _studentService = studentService;

        StudentsListView.ItemsSource = _studentService.PopulateStudentList();
    }

    private void ChangeStudentNameAndWorkInfo_Click (object sender, RoutedEventArgs e)
    {
        var selectedStudent = StudentsListView.SelectedItem as Student;

        if (selectedStudent == null)
        {
            MessageBox.Show("Please, select student from list first to update info", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return;
        }

        string changedStudentFullName = ChangeStudentFullNameTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(changedStudentFullName))
        {
            MessageBox.Show("Please, enter a valid student name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return;
        }

        if (IsWorkingComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            bool isWorkingInDepartment = selectedItem.Content.ToString() == "Yes";

            _studentService.ChangeStudentNameAndWorkInfo(selectedStudent, changedStudentFullName, isWorkingInDepartment);
            
            StudentsListView.ItemsSource = _studentService.PopulateStudentList();
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