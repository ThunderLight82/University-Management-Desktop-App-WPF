using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.GroupManagementService;

public partial class GroupManagementWindowDeleteGroupPage
{
    private GroupService _groupService;

    public GroupManagementWindowDeleteGroupPage(GroupService groupService)
    {
        InitializeComponent();

        _groupService = groupService;

        CourseComboBox.ItemsSource = _groupService.PopulateCourseList();

        CourseComboBox.SelectionChanged += ComboBoxRefreshAll;
    }

    private void DeleteGroup_Click(object sender, RoutedEventArgs e)
    {
        if (CourseComboBox.SelectedItem is Course selectedCourse)
        {
            string preferableGroupToDelete = DeleteGroupComboBox.SelectedItem as string;

            if (!string.IsNullOrWhiteSpace(preferableGroupToDelete))
            {
                var deleteGroup = selectedCourse.Groups.FirstOrDefault(group =>
                    group.GroupName == preferableGroupToDelete);

                if (deleteGroup!.Students.Count == 0)
                { 
                    _groupService.DeleteGroup(selectedCourse, preferableGroupToDelete);

                    ComboBoxRefreshAll(null!, null!);
                }
                else
                {
                    MessageBox.Show("Cannot delete this group because it contains students.\n" +
                                    "If you want to remove a group, please remove the active students within it in " +
                                    "\"Manage Students\" section before proceeding",
                        "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please, select a group from course to delete", 
                    "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select a course first to delete the group from", 
                "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ComboBoxRefreshAll(object sender, SelectionChangedEventArgs e)
    {
        if (CourseComboBox.SelectedItem is Course selectedCourse)
        {
            DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
        }
    }
}