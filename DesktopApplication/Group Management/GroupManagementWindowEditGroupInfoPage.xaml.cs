using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication.Group_Management;

public partial class GroupManagementWindowEditGroupInfoPage
{
    private UniversityDbContext _dbContext;

    public GroupManagementWindowEditGroupInfoPage(UniversityDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
        CourseComboBox.ItemsSource = _dbContext.Courses;
        // Maybe use "CourseComboBox.ItemsSource = _dbContext.Courses.Local" instead???
        SelectCuratorNameComboBox.ItemsSource = _dbContext.Teachers;
        // Maybe use "CourseComboBox.ItemsSource = _dbContext.Teachers.Local" instead???
        CourseComboBox.SelectionChanged += ComboBoxRefreshAll;
    }

    private void EditGroupName_Click(object sender, RoutedEventArgs e)
    {
        var selectedGroupName = EditGroupNameComboBox.SelectedItem as string;
        string newGroupName = EditGroupNameTextBox.Text.Trim();

        if (CourseComboBox.SelectedItem is not Course selectedCourse)
        {
            MessageBox.Show("Please, select a course first to edit the group name within", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    
        if (string.IsNullOrWhiteSpace(selectedGroupName) || string.IsNullOrWhiteSpace(newGroupName))
        {
            MessageBox.Show("Please, select a group from list and provide a new group name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    
        var editGroupName = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);
    
        editGroupName.GroupName = newGroupName;

        _dbContext.SaveChanges();

        EditGroupNameTextBox.Clear();

        ComboBoxRefreshAll(null, null);
    }
    
    private void SelectGroupCurator_Click(object sender, RoutedEventArgs e)
    {
        var selectedTeacher = SelectCuratorNameComboBox.SelectedItem as Teacher;
        var selectedGroupName = SelectGroupToAddCuratorComboBox.SelectedItem as string;
    
        if (string.IsNullOrWhiteSpace(selectedGroupName) || selectedTeacher != null)
        {
            var selectedCourse = CourseComboBox.SelectedItem as Course;
            
            if (selectedCourse != null)
            {
                var selectedGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);
    
                if (selectedGroup != null)
                {
                    selectedGroup.GroupCurator.Add(selectedTeacher);

                    _dbContext.SaveChanges();

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