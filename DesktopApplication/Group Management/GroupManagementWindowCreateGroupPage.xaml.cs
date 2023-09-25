using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class GroupManagementWindowCreateGroupPage : Page
{
    private DataRepository _dataRepository;

    public GroupManagementWindowCreateGroupPage(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
        CourseComboBox.ItemsSource = _dataRepository.Courses;
    }

    private void CreateGroup_Click(object sender, RoutedEventArgs e)
    {
        string groupName = GroupNameTextBox.Text.Trim();
    
        if (!string.IsNullOrWhiteSpace(groupName))
        {
            Course selectedCourse = CourseComboBox.SelectedItem as Course;
            
            if (selectedCourse != null)
            {
                bool groupNameExists = selectedCourse.Groups.Any(group => group.GroupName == groupName);
    
                if (!groupNameExists)
                {
                    selectedCourse.LastUsedGroupId++;
    
                    var createNewGroup = new Group
                    {
                        GroupId = selectedCourse.CourseId * 10 + selectedCourse.LastUsedGroupId,
                        GroupName = groupName
                    };
                        
                    selectedCourse.Groups.Add(createNewGroup);
    
                    _dataRepository.Groups.Add(createNewGroup);
    
                    GroupNameTextBox.Clear();
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
}
