using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class GroupManagementWindowDeleteGroupPage : Page
{
    private DataRepository _dataRepository;
    public GroupManagementWindowDeleteGroupPage(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
        CourseComboBox.ItemsSource = _dataRepository.Courses;
        CourseComboBox.SelectionChanged += ComboBoxRefreshAll;
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
                                        "\"Manage Students\" section before proceeding", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        selectedCourse.Groups.Remove(deleteGroup);
    
                        _dataRepository.Groups.Remove(deleteGroup);

                        ComboBoxRefreshAll(null, null);
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


    private void ComboBoxRefreshAll(object sender, SelectionChangedEventArgs e)
    {
        Course selectedCourse = CourseComboBox.SelectedItem as Course;
    
        if (selectedCourse != null)
        {
            DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
        }
    }
}