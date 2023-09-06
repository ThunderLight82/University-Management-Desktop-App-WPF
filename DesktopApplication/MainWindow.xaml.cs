using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class MainWindow : Window
{
    private readonly DataRepository _dataRepository;
    
    public MainWindow()
    {
        InitializeComponent();

        _dataRepository = new DataRepository();

        InitializeData();

        DataContext = _dataRepository.Courses;

        /*var courses = new List<Course>
        {
            new()
            {
                CourseId = 1,
                CourseName = "System Engineer",
                Groups = new List<Group>
                {
                    new ()
                    {
                        GroupId = 11,
                        GroupName = "SSE-22",
                        Students = new List<Student>
                        {
                            new() {StudentId = 0, StudentFullName = "FillFullName", isWorkingInDepartment = false},
                            new() {StudentId = 0, StudentFullName = "FillFullName", isWorkingInDepartment = false},
                            new() {StudentId = 0, StudentFullName = "FillFullName", isWorkingInDepartment = false},
                            new() {StudentId = 0, StudentFullName = "FillFullName", isWorkingInDepartment = false}
                        }
                    },
                    new ()
                    {
                        GroupId = 12,
                        GroupName = "SSE-11",
                        Students = new List<Student>
                        {
                            new() {StudentId = 111, StudentFullName = "FillFullName", isWorkingInDepartment = false},
                            new() {StudentId = 111, StudentFullName = "FillFullName", isWorkingInDepartment = false},
                            new() {StudentId = 111, StudentFullName = "FillFullName", isWorkingInDepartment = false},
                            new() {StudentId = 111, StudentFullName = "FillFullName", isWorkingInDepartment = false}
                        }
                    },
                    new ()
                    {
                        GroupId = 13,
                        GroupName = "SSE-12",
                        Students = new List<Student>
                        {
                            new() {StudentId = 111, StudentFullName = "FillFullName", isWorkingInDepartment = false},
                            new() {StudentId = 111, StudentFullName = "FillFullName", isWorkingInDepartment = true},
                            new() {StudentId = 111, StudentFullName = "FillFullName", isWorkingInDepartment = true},
                            new() {StudentId = 111, StudentFullName = "FillFullName", isWorkingInDepartment = false}
                        }
                    },
                }
            },
            new()
            {
                CourseId = 2,
                CourseName = "Software Engineer",
                Groups = new List<Group>
                {
                    new() {GroupId = 21, GroupName = "SWE-11"},
                    new() {GroupId = 22, GroupName = "SWE-12"},
                    new() {GroupId = 23, GroupName = "SWE-13"},
                    new() {GroupId = 24, GroupName = "SWE-31"},
                    new() {GroupId = 25, GroupName = "SWE-32"},
                    new() {GroupId = 26, GroupName = "SWE-21"},
                    new() {GroupId = 27, GroupName = "SWE-22"}
                }
            },
            
            new()
            {
                CourseId = 3,
                CourseName = "Data Science",
                Groups = new List<Group>
                {
                    new() {GroupId = 31, GroupName = "DS-41"},
                    new() {GroupId = 32, GroupName = "DS-31"},
                    new() {GroupId = 33, GroupName = "DS-11"},
                    new() {GroupId = 34, GroupName = "DS-12"}
                }
            },
            
            new()
            {
                CourseId = 4,
                CourseName = "Data Analysis",
                Groups = new List<Group>
                {
                    new() {GroupId = 41, GroupName = "DA-31"},
                    new() {GroupId = 42, GroupName = "DA-32"},
                    new() {GroupId = 43, GroupName = "DA-21"}
                }
            },
            
            new()
            {
                CourseId = 5,
                CourseName = "Cyber Security",
                Groups = new List<Group>
                {
                    new() {GroupId = 51, GroupName = "CS-21"}
                }
            },
        };*/

        CourseListView.SelectionChanged += CourseListView_SelectionChanged;
        
        GroupListView.SelectionChanged += GroupListView_SelectionChanged;
    }

    private void InitializeData()
    {
        Course course = new Course
        {
            CourseId = 1,
            CourseName = "System Engineer"
        };

        Group group = new Group
        {
            GroupId = 11,
            GroupName = "SSE-22"
        };

        Student student1 = new Student
        {
            StudentId = 1,
            StudentFullName = "Student 1",
            isWorkingInDepartment = false
        };
        
        group.Students.Add(student1);
        
        course.Groups.Add(group);
        
        _dataRepository.Courses.Add(course);
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
}