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

    private async void ChangeStudentNameAndWorkInfoAsync_Click (object sender, RoutedEventArgs e)
    {
        if (StudentsListView.SelectedItem is Student selectedStudent)
        {
            string changedStudentFullName = ChangeStudentFullNameTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(changedStudentFullName))
            {
                if (IsWorkingComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    bool isWorkingInDepartment = selectedItem.Content.ToString() == "Yes";

                    await _studentService.ChangeStudentNameAndWorkInfoAsync(selectedStudent, changedStudentFullName, isWorkingInDepartment);

                    StudentsListView.ItemsSource = _studentService.PopulateStudentList();
                }
            }
            else
            {
                MessageBox.Show("Please, enter a valid student name", 
                    "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select student from list first to update info", 
                "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
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