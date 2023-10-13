using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DesktopApplication.Entities;

namespace DesktopApplication.GroupManagementService;

public partial class GroupManagementWindowEditGroupInfoPage
{
    private UniversityDbContext _dbContext;
    private GroupService _groupService;

    public GroupManagementWindowEditGroupInfoPage(UniversityDbContext dbContext, GroupService groupService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _groupService = groupService;

        CourseComboBox.ItemsSource = _dbContext.Courses.Local.ToObservableCollection();

        SelectCuratorNameComboBox.ItemsSource = _dbContext.Teachers.Local.ToObservableCollection();

        CourseComboBox.SelectionChanged += ComboBoxRefreshAll;
    }

    private void EditGroupName_Click(object sender, RoutedEventArgs e)
    {
        if (CourseComboBox.SelectedItem is Course selectedCourse)
        {
            var selectedGroupName = EditGroupNameComboBox.SelectedItem as string;

            if (!string.IsNullOrWhiteSpace(selectedGroupName))
            {
                string newGroupName = EditGroupNameTextBox.Text.Trim();

                bool groupNameChanged = _groupService.EditGroupName(selectedCourse, selectedGroupName, newGroupName);

                if (groupNameChanged)
                {
                    ComboBoxRefreshAll(null, null);

                    EditGroupNameTextBox.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please, select a group from list to provide a new group name", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                EditGroupNameTextBox.Clear();
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
        if (CourseComboBox.SelectedItem is Course selectedCourse)
        {
            var selectedTeacher = SelectCuratorNameComboBox.SelectedItem as Teacher;
            var selectedGroupName = SelectGroupToAddCuratorComboBox.SelectedItem as string;

            bool curatorAssigned = _groupService.SelectGroupCurator(selectedCourse, selectedTeacher, selectedGroupName);

            if (curatorAssigned)
            {
                SelectGroupToAddCuratorComboBox.SelectedIndex = -1;
                SelectCuratorNameComboBox.SelectedIndex = -1;
            }
        }
        else
        {
            MessageBox.Show("Please, select a course first to edit teacher curation in group", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void SelectGroupToAddCuratorInfoForComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedGroup = GetSelectedGroupCurationInfo();

        if (selectedGroup != null)
        {
            if (selectedGroup.GroupCurator.Any())
            {
                string curatorName = string.Join(", ", selectedGroup.GroupCurator.Select(curator => curator.TeacherFullName));
                CurationInfoTextBlock.Text = $"This group already has curators named: {curatorName}";
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

    private void EditGroupNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (EditGroupNameComboBox.SelectedItem is string selectedGroupName)
        {
            EditGroupNameTextBox.Text = selectedGroupName;
        }
    }


    private Group GetSelectedGroupCurationInfo()
    {
        var selectedGroupName = SelectGroupToAddCuratorComboBox.SelectedItem as string;
        var selectedCourse = CourseComboBox.SelectedItem as Course;
    
        if (selectedCourse != null && !string.IsNullOrWhiteSpace(selectedGroupName))
        {
            return selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);
        }
    
        return null;
    }

    private void ComboBoxRefreshAll(object sender, SelectionChangedEventArgs e)
    {
        if (CourseComboBox.SelectedItem is Course selectedCourse)
        {
            EditGroupNameComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
            SelectGroupToAddCuratorComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
            SelectCuratorNameComboBox.SelectedItem = null;
        }
    }
}