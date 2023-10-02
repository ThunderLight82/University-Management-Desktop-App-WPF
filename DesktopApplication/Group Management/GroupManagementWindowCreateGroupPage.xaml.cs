﻿using System.Linq;
using System.Windows;

namespace DesktopApplication.Group_Management;

public partial class GroupManagementWindowCreateGroupPage
{
    private UniversityDbContext _dbContext;

    public GroupManagementWindowCreateGroupPage(UniversityDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
        CourseComboBox.ItemsSource = _dbContext.Courses;
        // Maybe use "CourseComboBox.ItemsSource = _dbContext.Courses.Local" instead???
    }

    private void CreateGroup_Click(object sender, RoutedEventArgs e)
    {
        string groupName = GroupNameTextBox.Text.Trim();

        if (!string.IsNullOrWhiteSpace(groupName))
        {
            if (CourseComboBox.SelectedItem is Course selectedCourse)
            {
                var groupNameExists = selectedCourse.Groups.Any(group => group.GroupName == groupName);

                if (!groupNameExists)
                {
                    selectedCourse.LastUsedGroupId++;

                    var createNewGroup = new Group
                    {
                        GroupId = selectedCourse.CourseId * 10 + selectedCourse.LastUsedGroupId,
                        GroupName = groupName
                    };

                    selectedCourse.Groups.Add(createNewGroup);

                    _dbContext.Groups.Add(createNewGroup);

                    GroupNameTextBox.Clear();
                }
                else
                {
                    MessageBox.Show(
                        "A group with same name already exist. Please, use other name for new group to create", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please, enter a valid group name", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select a course first to add group to it", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}