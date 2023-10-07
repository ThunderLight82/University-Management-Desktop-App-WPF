using System.Windows;
using DesktopApplication.Entities;

namespace DesktopApplication.GroupManagementService;

public partial class GroupManagementWindowCreateGroupPage
{
    private UniversityDbContext _dbContext;
    private GroupService _groupService;

    public GroupManagementWindowCreateGroupPage(UniversityDbContext dbContext, GroupService groupService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _groupService = groupService;

        CourseComboBox.ItemsSource = _dbContext.Courses.Local.ToObservableCollection();
    }

    private void CreateGroup_Click(object sender, RoutedEventArgs e)
    {
        if (CourseComboBox.SelectedItem is Course selectedCourse)
        {
            string groupName = GroupNameTextBox.Text.Trim();

            if (_groupService.CreateGroup(selectedCourse, groupName))
            {
                GroupNameTextBox.Clear();
            }
        }
        else
        {
            MessageBox.Show("Please, select a course first to add group to it", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}