using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DesktopApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesktopApplication.StudentManagementService;

public partial class StudentManagementWindowManageStudentsGroupPage
{
    private UniversityDbContext _dbContext;
    private HashSet<Student> _assignedStudents;

    public StudentManagementWindowManageStudentsGroupPage(UniversityDbContext dbContext, HashSet<Student> assignedStudents)
    {
        InitializeComponent();
        _assignedStudents = assignedStudents;
        _dbContext = dbContext;
        CourseComboBox.ItemsSource = _dbContext.Courses;
        StudentsListView.ItemsSource = _dbContext.Students;
    }

    private void AddStudentsToGroup_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (selectedCourse != null && !string.IsNullOrEmpty(selectedGroupName))
        {
            var selectedGroup = _dbContext.Groups
                .Include(group => group.Students)
                .FirstOrDefault(group => group.GroupName == selectedGroupName);

            if (selectedGroup != null)
            {
                var selectedStudents = StudentsListView.SelectedItems.OfType<Student>().ToList();

                if (selectedStudents.Count > 0)
                {
                    foreach (var student in selectedStudents)
                    {
                        if (student.CurrentGroupName == null && !_assignedStudents.Contains(student))
                        {
                            student.CurrentGroupName = selectedGroup.GroupName;
                            selectedGroup.Students.Add(student);

                            _assignedStudents.Add(student);
                        }
                        else
                        {
                            MessageBox.Show("Student '" + student.StudentFullName + "' is already assigned to a group '" + student.CurrentGroupName + "'", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    _dbContext.SaveChanges();

                    StudentsListView.Items.Refresh();

                    StudentsListView.SelectedItems.Clear();
                }
                else
                {
                    MessageBox.Show("Please, select a student from the list to add to the group", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("The selected group does not exist for the chosen course", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        else
        {
            MessageBox.Show("Please, select both a course and a group to add students to", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DeleteStudentsFromGroup_Click(object sender, RoutedEventArgs e)
    {
        var selectedStudents = StudentsListView.SelectedItems.OfType<Student>().ToList();

        if (selectedStudents.Count > 0)
        {
            foreach (var student in selectedStudents)
            {
                var selectedGroup = _dbContext.Groups
                    .Include(group => group.Students)
                    .FirstOrDefault(group => group.Students.Contains(student));

                if (selectedGroup != null)
                {
                    selectedGroup.Students.Remove(student);
                    student.CurrentGroupName = null;

                    _assignedStudents.Remove(student);
                }
                else
                {
                    MessageBox.Show("This student is not assigned to any group", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            StudentsListView.Items.Refresh();

            StudentsListView.SelectedItems.Clear();
        }
        else
        {
            MessageBox.Show("Please, select a student from the list to remove it from the group", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;

        GroupComboBox.ItemsSource = selectedCourse?.Groups.Select(group => group.GroupName).ToList();
    }
}