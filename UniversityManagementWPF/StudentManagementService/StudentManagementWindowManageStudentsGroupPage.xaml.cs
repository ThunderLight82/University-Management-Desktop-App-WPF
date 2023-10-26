using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.StudentManagementService;

public partial class StudentManagementWindowManageStudentsGroupPage
{
    private StudentService _studentService;

    public StudentManagementWindowManageStudentsGroupPage(StudentService studentService)
    {
        InitializeComponent();

        _studentService = studentService;

        CourseComboBox.ItemsSource = _studentService.PopulateCourseList();
        StudentsListView.ItemsSource = _studentService.PopulateStudentList();
    }
    private void AddStudentsToGroup_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (selectedCourse != null && !string.IsNullOrWhiteSpace(selectedGroupName))
        {
            var selectedStudents = StudentsListView.SelectedItems.OfType<Student>().ToList();

            if (selectedStudents.Count != 0)
            {
                foreach (var student in selectedStudents)
                {
                    if (!string.IsNullOrWhiteSpace(student.StudentFullName))
                    {
                        if (string.IsNullOrWhiteSpace(student.CurrentGroupName))
                        {
                            _studentService.AddStudentsToGroup(selectedCourse, selectedGroupName, selectedStudents);

                            StudentsListView.ItemsSource = _studentService.PopulateStudentList();

                            StudentsListView.SelectedItems.Clear();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Student '" + student.StudentFullName + "' is already assigned to a group '" +
                                student.CurrentGroupName + "'", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);

                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "Selected student have no name or whitespace in name field. Please, provide a valid name for students", 
                            "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please, select a student from the list to add to the group", 
                    "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select both a course and a group to add students to", 
                "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    private void DeleteStudentsFromGroup_Click(object sender, RoutedEventArgs e)
    {
        var selectedStudents = StudentsListView.SelectedItems.OfType<Student>().ToList();

        if (selectedStudents.Count != 0)
        {
            foreach (var student in selectedStudents)
            {
                if (!string.IsNullOrWhiteSpace(student.StudentFullName))
                {
                    if (!string.IsNullOrWhiteSpace(student.CurrentGroupName))
                    {
                        _studentService.DeleteStudentsFromGroup(selectedStudents);

                        StudentsListView.ItemsSource = _studentService.PopulateStudentList();

                        StudentsListView.SelectedItems.Clear();
                    }
                    else
                    {
                        MessageBox.Show("This student is not assigned to any group",
                            "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);

                        return;
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Selected student have no name or whitespace in name field. Please, provide a valid name for students",
                        "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please, select a student from the list to remove from the group",
                "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;

        GroupComboBox.ItemsSource = selectedCourse?.Groups.Select(group => group.GroupName).ToList();
    }
}