using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class MainWindow : Window
{
    private DataRepository _dataRepository;
    
    public MainWindow()
    {
        InitializeComponent();

        _dataRepository = new DataRepository();

        InitializeData();

        DataContext = _dataRepository.Courses;

        CourseListView.SelectionChanged += CourseListView_SelectionChanged;
        
        GroupListView.SelectionChanged += GroupListView_SelectionChanged;
    }

    private void InitializeData()
    {
        var courses = new List<Course>
        {
            new() { CourseId = 1, CourseName = "System Engineer" },
            new() { CourseId = 2, CourseName = "Software Engineer" },
            new() { CourseId = 3, CourseName = "Data Science" },
            new() { CourseId = 4, CourseName = "Data Analysis"},
            new() { CourseId = 5, CourseName = "Cyber Security"}
        };

        // VVV Hardcoded group and student below only for testing purpose VVV
        
        Group group = new Group
        {
            GroupId = 11,
            GroupName = "Test Group"
        };

        Student student1 = new Student
        {
            StudentId = 1,
            StudentFullName = "Test Student 1",
            isWorkingInDepartment = false
        };
        
        group.Students.Add(student1);
        
        foreach (var course in courses)
        {
            course.Groups.Add(group);
            _dataRepository.Courses.Add(course);
        }
    }

    private void CourseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CourseListView.SelectedItem != null)
        {
            var selectedCourse = (Course)CourseListView.SelectedItem;

            GroupListView.ItemsSource = selectedCourse.Groups;
        }
        else
        {
            GroupListView.ItemsSource = null;
        }
    }

    private void GroupListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (GroupListView.SelectedItem != null)
        {
            var selectedGroup = (Group)GroupListView.SelectedItem;

            StudentListView.ItemsSource = selectedGroup.Students;
        }
        else
        {
            StudentListView.ItemsSource = null;
        }
    }

    private void OpenGroupManagementWindow_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            GroupManagementWindow groupManagementWindow = new GroupManagementWindow(_dataRepository);
            groupManagementWindow.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void RefreshData_Click(object sender, RoutedEventArgs e)
    {
        CourseListView.SelectedIndex = -1;
        GroupListView.SelectedIndex = -1;
        StudentListView.SelectedIndex = -1;
    }
}