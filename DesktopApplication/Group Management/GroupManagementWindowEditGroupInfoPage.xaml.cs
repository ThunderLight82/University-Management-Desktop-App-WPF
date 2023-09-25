using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class GroupManagementWindowEditGroupInfoPage : Page
{
    private DataRepository _dataRepository;
    public GroupManagementWindowEditGroupInfoPage(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
        CourseComboBox.ItemsSource = _dataRepository.Courses;
        SelectCuratorNameComboBox.ItemsSource = _dataRepository.Teachers;
        CourseComboBox.SelectionChanged += ComboBoxRefreshAll;
    }

    private void EditGroupName_Click(object sender, RoutedEventArgs e)
    {
        string selectedGroupName = EditGroupNameComboBox.SelectedItem as string;
        string newGroupName = EditGroupNameTextBox.Text.Trim();
    
        Course selectedCourse = CourseComboBox.SelectedItem as Course;
    
        if (selectedCourse == null)
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
    
        Group editGroupName = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);
    
        editGroupName.GroupName = newGroupName;
    
        EditGroupNameTextBox.Clear();

        ComboBoxRefreshAll(null, null);
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
                    selectedGroup.GroupCurator.Add(selectedTeacher);

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
        Group selectedGroup = GetSelectedGroupCurationInfo();
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
        string selectedGroupName = SelectGroupToAddCuratorComboBox.SelectedItem as string;
        Course selectedCourse = CourseComboBox.SelectedItem as Course;
    
        if (selectedCourse != null && !string.IsNullOrWhiteSpace(selectedGroupName))
        {
            return selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);
        }
    
        return null;
    }

    private void ComboBoxRefreshAll(object sender, SelectionChangedEventArgs e)
    {
        Course selectedCourse = CourseComboBox.SelectedItem as Course;
    
        if (selectedCourse != null)
        {
            EditGroupNameComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
            SelectGroupToAddCuratorComboBox.ItemsSource = selectedCourse.Groups.Select(group => group.GroupName).ToList();
            SelectCuratorNameComboBox.SelectedItem = null;
        }
    }
}