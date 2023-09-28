using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DesktopApplication.Group_Management;
using DesktopApplication.Student_Management;
using DesktopApplication.Teacher_Management;

namespace DesktopApplication;

public partial class MainWindow
{
    private UniversityDbContext _dbContext;

    public MainWindow()
    {
        InitializeComponent();

        _dbContext = (UniversityDbContext)Application.Current.Resources["UniversityDbContext"];

        var courses = _dbContext.Courses.Local.ToObservableCollection();
        CourseListView.ItemsSource = courses;

        CourseListView.SelectionChanged += CourseListView_SelectionChanged;
        
        GroupListView.SelectionChanged += GroupListView_SelectionChanged;
    }

    private void CourseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CourseListView.SelectedItem != null) 
        {
            var selectedCourse = (Course)CourseListView.SelectedItem;

            GroupListView.ItemsSource = _dbContext.Groups.Where(group =>
                group.CourseId == selectedCourse.CourseId).ToList();
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

            StudentListView.ItemsSource = _dbContext.Students.Where(student =>
                student.GroupId == selectedGroup.GroupId).ToList();
        }
        else
        {
            StudentListView.ItemsSource = null;
        }
    }

    // private void OpenGroupManagementWindow_Click(object sender, RoutedEventArgs e)
    // {
    //     var groupManagementWindow = new GroupManagementWindow(_dbContext);
    //     groupManagementWindow.ShowDialog();
    // }
    //
    // private void OpenStudentManagementWindow_Click(object sender, RoutedEventArgs e)
    // {
    //     var studentManagementWindow = new StudentManagementWindow(_dbContext, new HashSet<Student>());
    //     studentManagementWindow.ShowDialog();
    // }
    //
    // private void OpenTeacherManagementWindow_Click(object sender, RoutedEventArgs e)
    // {
    //     var teacherManagementWindow = new TeacherManagementWindow(_dbContext);
    //     teacherManagementWindow.ShowDialog();
    // }

     // private void RefreshData_Click(object sender, RoutedEventArgs e)
     // {
     //     CourseListView.SelectedIndex = -1;
     //     GroupListView.SelectedIndex = -1;
     //     StudentListView.SelectedIndex = -1;
     // }
}