using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class MainWindow : Window
{
    private DataRepository _dataRepository = App.DataRepository;

    public MainWindow()
    {
        InitializeComponent();

        CourseListView.ItemsSource = _dataRepository.Courses;

        DataContext = _dataRepository.Courses;

        CourseListView.SelectionChanged += CourseListView_SelectionChanged;
        
        GroupListView.SelectionChanged += GroupListView_SelectionChanged;
    }

    private void CourseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CourseListView.SelectedItem != null) 
        {
            var selectedCourse = (Course)CourseListView.SelectedItem;

            GroupListView.ItemsSource = selectedCourse.Groups;
        }
        else
        {
            GroupListView.ItemsSource = null;
        }
    }

    private void GroupListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (GroupListView.SelectedItem != null)
        {
            var selectedGroup = (Group)GroupListView.SelectedItem;

            StudentListView.ItemsSource = selectedGroup.Students;
        }
        else
        {
            StudentListView.ItemsSource = null;
        }
    }

    private void OpenGroupManagementWindow_Click(object sender, RoutedEventArgs e)
    {
        var groupManagementWindow = new GroupManagementWindow(_dataRepository);
        groupManagementWindow.ShowDialog();
    }
    
    private void OpenStudentManagementWindow_Click(object sender, RoutedEventArgs e)
    {
        var studentManagementWindow = new StudentManagementWindow(_dataRepository, new HashSet<Student>());
        studentManagementWindow.ShowDialog();
    }

    private void RefreshData_Click(object sender, RoutedEventArgs e)
    {
        CourseListView.SelectedIndex = -1;
        GroupListView.SelectedIndex = -1;
        StudentListView.SelectedIndex = -1;
    }
}