using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class GroupManagementWindow : Window
{
    private DataRepository _dataRepository;
    
    public GroupManagementWindow(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
        CourseComboBox.ItemsSource = _dataRepository.Courses;
        SelectCuratorNameComboBox.ItemsSource = _dataRepository.Teachers;
        CourseComboBox.SelectionChanged += CourseComboBox_SelectionChanged;
    }

    private void EditGroupName_Click(object sender, RoutedEventArgs e)
    {
        string selectedGroupName = EditGroupNameComboBox.SelectedItem as string;
        string newGroupName = EditGroupNameTextBox.Text;

        if (!string.IsNullOrWhiteSpace(selectedGroupName) && !string.IsNullOrWhiteSpace(newGroupName))
        {
            Course selectedCourse = CourseComboBox.SelectedItem as Course;
           
            if (selectedCourse != null)
            {
                Group editGroupName = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);
               
                if (editGroupName != null)
                {
                    editGroupName.GroupName = newGroupName;

                    EditGroupNameTextBox.Clear();
                    
                    DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                    EditGroupNameComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                    SelectGroupToAddCuratorComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                }
            }
            else
            {
                MessageBox.Show("Please, select a group from list and provide a new group name", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select a course first to edit the group name within", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    private void SelectGroupCurator_Click(object sender, RoutedEventArgs e)
    {
        Teacher selectedTeacher = SelectCuratorNameComboBox.SelectedItem as Teacher;
        string selectedGroupName = SelectGroupToAddCuratorComboBox.SelectedItem as string;

        if (string.IsNullOrWhiteSpace(selectedGroupName) || selectedTeacher != null)
        {
            Course selectedCourse = CourseComboBox.SelectedItem as Course;
            
            if (selectedCourse != null)
            {
                Group selectedGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);

                if (selectedGroup != null)
                {
                    selectedGroup.GroupCuratorName = selectedTeacher.TeacherFullName;
                    
                    SelectGroupToAddCuratorComboBox.SelectedIndex = -1;
                    SelectCuratorNameComboBox.SelectedIndex = -1;
                }
            }
            else
            {
                MessageBox.Show("Please, select a course to assign the curator/teacher to a group", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select both a group and a teacher to apply the selection", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void CreateGroup_Click(object sender, RoutedEventArgs e)
    {
        string groupName = GroupNameTextBox.Text;

        if (!string.IsNullOrWhiteSpace(groupName))
        {
            Course selectedCourse = CourseComboBox.SelectedItem as Course;
            
            if (selectedCourse != null)
            {
                bool groupNameExists = selectedCourse.Groups.Any(group => group.GroupName == groupName);
                
                if(!groupNameExists)
                {
                    selectedCourse.LastUsedGroupId++;

                    Group createNewGroup = new Group
                    {
                        GroupId = selectedCourse.CourseId * 10 + selectedCourse.LastUsedGroupId,
                        GroupName = groupName
                    };
                        
                    selectedCourse.Groups.Add(createNewGroup);
                
                    GroupNameTextBox.Clear();
                
                    DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                    EditGroupNameComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                    SelectGroupToAddCuratorComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                }
                else
                {
                    MessageBox.Show("A group with same name already exist. Please, use other name for new group to create", "Error", 
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

    private void DeleteGroup_Click(object sender, RoutedEventArgs e)
    {
        string preferableGroupToDelete = DeleteGroupComboBox.SelectedItem as string;

        if (!string.IsNullOrWhiteSpace(preferableGroupToDelete))
        {
            Course selectedCourse = CourseComboBox.SelectedItem as Course;

            if (selectedCourse != null)
            {
                Group deleteGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == preferableGroupToDelete);
                
                if (deleteGroup != null)
                {
                    if (deleteGroup.Students.Count > 0)
                    {
                        MessageBox.Show("Cannot delete this group because it contains students.\n" +
                                        "If you want to remove a group, please remove the active students within it in " +
                                        "\"Manage Student\" section before proceeding", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        selectedCourse.Groups.Remove(deleteGroup);

                        DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                        EditGroupNameComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                        SelectGroupToAddCuratorComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please, select a group from course to delete", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select a course first to delete the group from", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void SelectGroupToAddCuratorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Group selectedGroup = GetSelectedGroupInfo();
        if (selectedGroup != null)
        {
            if (!string.IsNullOrWhiteSpace(selectedGroup.GroupCuratorName))
            {
                CurationInfoTextBlock.Text = $"This group already has a curator named {selectedGroup.GroupCuratorName}]";
            }
            else
            {
                CurationInfoTextBlock.Text = "This group didn't have an assigned curator for now";
            }
        }
        else
        {
            CurationInfoTextBlock.Text = string.Empty;
        }
    }
    
    private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Course selectedCourse = CourseComboBox.SelectedItem as Course;

        if (selectedCourse != null)
        {
            DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
            EditGroupNameComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
            SelectGroupToAddCuratorComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
            SelectCuratorNameComboBox.SelectedItem = null;
        }
    }

    private Group GetSelectedGroupInfo()
    {
        string selectedGroupName = SelectGroupToAddCuratorComboBox.SelectedItem as string;
        Course selectedCourse = CourseComboBox.SelectedItem as Course;

        if (selectedCourse != null && !string.IsNullOrWhiteSpace(selectedGroupName))
        {
            return selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);
        }

        return null;
    }

    private void RefreshAllGroupComboBoxes()
    {
        // Need to add later a future realization of sorta refresher to all group combo boxes smth like "CourseComboBox_SelectionChanged"
    }
}