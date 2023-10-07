using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DesktopApplication.Entities;
using DesktopApplication.GroupManagementService;
using DesktopApplication.StudentManagementService;
using DesktopApplication.TeacherManagementService;
using Microsoft.EntityFrameworkCore;

namespace DesktopApplication;

public partial class MainWindow
{
    private UniversityDbContext _dbContext;
    private GroupService _groupService;
    public MainWindow(UniversityDbContext dbContext)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _groupService = new GroupService(_dbContext);

        LoadEntitiesData();

        CourseListView.ItemsSource = _dbContext.Courses.Local.ToObservableCollection();

        CourseListView.SelectionChanged += CourseListView_SelectionChanged;
        
        GroupListView.SelectionChanged += GroupListView_SelectionChanged;
    }

    private void CourseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CourseListView.SelectedItem != null)
        {
            var selectedCourse = (Course)CourseListView.SelectedItem;

            _dbContext.Entry(selectedCourse).Collection(col => col.Groups).Load();

            GroupListView.ItemsSource = selectedCourse.Groups.ToList();
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

            _dbContext.Entry(selectedGroup).Collection(col => col.Students).Load();

            StudentListView.ItemsSource = selectedGroup.Students.ToList();
        }
        else
        {
            StudentListView.ItemsSource = null;
        }
    }

    private void OpenGroupManagementWindow_Click(object sender, RoutedEventArgs e)
    {
        var groupManagementWindow = new GroupManagementWindow(_dbContext, _groupService);
        groupManagementWindow.ShowDialog();
    }
    
    private void OpenStudentManagementWindow_Click(object sender, RoutedEventArgs e)
    {
        var studentManagementWindow = new StudentManagementWindow(_dbContext, new HashSet<Student>());
        studentManagementWindow.ShowDialog();
    }
    
    private void OpenTeacherManagementWindow_Click(object sender, RoutedEventArgs e)
    {
        var teacherManagementWindow = new TeacherManagementWindow(_dbContext);
        teacherManagementWindow.ShowDialog();
    }

     private void RefreshData_Click(object sender, RoutedEventArgs e)
     {
         CourseListView.SelectedIndex = -1;
         GroupListView.SelectedIndex = -1;
         StudentListView.SelectedIndex = -1;
         LoadEntitiesData();
     }

     private void LoadEntitiesData()
     {
         _dbContext.Courses.Load();
         _dbContext.Groups.Load();
         _dbContext.Students.Load();
         _dbContext.Teachers.Load();
     }
}