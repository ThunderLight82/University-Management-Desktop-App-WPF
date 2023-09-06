using System.Linq;
using System.Windows;

namespace DesktopApplication;

public partial class GroupManagementWindow : Window
{

    private DataRepository _dataRepository;
    
    public GroupManagementWindow(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
        InitializeComboBoxes();
    }

    /*private void EditGroup_Click(object sender, RoutedEventArgs e)
   {
       string selectedGroup = GroupComboBox.SelectedItem as string;
       string newGroupName = NewGroupNameTextBox.Text;

       if (!string.IsNullOrWhiteSpace(selectedGroup) && !string.IsNullOrWhiteSpace(newGroupName))
       {
           Course selectedCourse = CourseComboBox.SelectedItem as Course;

           if (selectedCourse != null)
           {
               Group editGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroup);

               if (editGroup != null)
               {
                   editGroup.GroupName = newGroupName;

                   GroupComboBox.SelectedIndex = -1;
                   
                   NewGroupNameTextBox.Clear();
               }
           }
       }
       else
       {
           MessageBox.Show("Please select a group and provide a new group name", "Error", 
               MessageBoxButton.OK, MessageBoxImage.Error);
       }

   }*/
    
    private void CreateGroup_Click(object sender, RoutedEventArgs e)
    {
        string groupName = GroupNameTextBox.Text;

        if (!string.IsNullOrWhiteSpace(groupName))
        {
            Course selectedCourse = CourseComboBox.SelectedItem as Course;
            
            if (selectedCourse != null)
            {
                Group createNewGroup = new Group { GroupName = groupName };
                selectedCourse.Groups.Add(createNewGroup);
                
                GroupNameTextBox.Clear();
                
                InitializeComboBoxes();
            }
            else
            {
                MessageBox.Show("Please select a course to add group to it", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please enter a valid group name", "Error", 
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
                    selectedCourse.Groups.Remove(deleteGroup);

                    InitializeComboBoxes();
                }
            }
        }
        else
        {
            MessageBox.Show("Please select a group to delete", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void InitializeComboBoxes()
    {
        CourseComboBox.ItemsSource = _dataRepository.Courses;
        
        foreach (var group in _dataRepository.Courses.SelectMany(course => course.Groups))
        {
            GroupComboBox.Items.Add(group.GroupName);
            DeleteGroupComboBox.Items.Add(group.GroupName);
        }
    }
}

/*private void UpdateGroupComboBoxes()
{
    GroupComboBox.Items.Clear();
    
    DeleteGroupComboBox.Items.Clear();

    foreach (Group group in groupList)
    {
        GroupComboBox.Items.Add(group.GroupName);
        DeleteGroupComboBox.Items.Add(group.GroupName);
    }
}*/