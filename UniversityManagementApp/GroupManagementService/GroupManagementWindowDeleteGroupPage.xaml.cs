using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UniversityManagement.Data;
using UniversityManagement.Entities;

namespace UniversityManagement.GroupManagementService;

public partial class GroupManagementWindowDeleteGroupPage
{
    private UniversityDbContext _dbContext;
    private GroupService _groupService;

    public GroupManagementWindowDeleteGroupPage(UniversityDbContext dbContext, GroupService groupService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _groupService = groupService;

        CourseComboBox.ItemsSource = _dbContext.Courses.Local.ToObservableCollection();

        CourseComboBox.SelectionChanged += ComboBoxRefreshAll;
    }

    private void DeleteGroup_Click(object sender, RoutedEventArgs e)
    {
        if (CourseComboBox.SelectedItem is Course selectedCourse)
        {
            string preferableGroupToDelete = DeleteGroupComboBox.SelectedItem as string;

            if (!string.IsNullOrWhiteSpace(preferableGroupToDelete))
            {
                bool groupDeleted = _groupService.DeleteGroup(selectedCourse, preferableGroupToDelete);

                if (groupDeleted)
                {
                    ComboBoxRefreshAll(null, null);
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
        if (CourseComboBox.SelectedItem is Course selectedCourse)
        {
            DeleteGroupComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
        }
    }
}