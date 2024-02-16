using System.Linq;
using System.Windows;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.GroupManagementService;

public partial class GroupManagementWindowCreateGroupPage
{
    private GroupService _groupService;

    public GroupManagementWindowCreateGroupPage(GroupService groupService)
    {
        InitializeComponent();

        _groupService = groupService;

        CourseComboBox.ItemsSource = _groupService.PopulateCourseList();
    }

    private async void CreateGroupAsync_Click(object sender, RoutedEventArgs e)
    {
        if (CourseComboBox.SelectedItem is Course selectedCourse)
        {
            string groupName = GroupNameTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var groupNameExist = selectedCourse.Groups.Any(group =>
                    group.GroupName == groupName);

                if (!groupNameExist)
                {
                    await _groupService.CreateGroupAsync(selectedCourse, groupName);

                    GroupNameTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("A group with the same name already exists. Please, use another name for the new group.",
                        "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please, enter a valid group name", 
                    "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        else
        {
            MessageBox.Show("Please, select a course first to add group to it", 
                "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}