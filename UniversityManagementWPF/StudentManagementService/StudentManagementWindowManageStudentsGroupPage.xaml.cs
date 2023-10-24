using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.StudentManagementService;

public partial class StudentManagementWindowManageStudentsGroupPage
{
    private UniversityDbContext _dbContext;
    private StudentService _studentService;

    public StudentManagementWindowManageStudentsGroupPage(UniversityDbContext dbContext, StudentService studentService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _studentService = studentService;

        CourseComboBox.ItemsSource = _dbContext.Courses.Local.ToObservableCollection();
        StudentsListView.ItemsSource = _dbContext.Students.Local.ToObservableCollection();
    }

    private void AddStudentsToGroup_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        var selectedStudents = StudentsListView.SelectedItems.OfType<Student>().ToList();

        if (_studentService.AddStudentsToGroup(selectedCourse, selectedGroupName, selectedStudents))
        {
            StudentsListView.Items.Refresh();

            StudentsListView.SelectedItems.Clear();
        }
    }

    private void DeleteStudentsFromGroup_Click(object sender, RoutedEventArgs e)
    {
        var selectedStudents = StudentsListView.SelectedItems.OfType<Student>().ToList();

        if (_studentService.DeleteStudentsFromGroup(selectedStudents))
        {
            StudentsListView.Items.Refresh();

            StudentsListView.SelectedItems.Clear();
        }
    }

    private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;

        GroupComboBox.ItemsSource = selectedCourse?.Groups.Select(group => group.GroupName).ToList();
    }
}