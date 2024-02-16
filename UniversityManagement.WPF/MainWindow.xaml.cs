using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;
using UniversityManagement.WPF.GroupManagementService;
using UniversityManagement.WPF.StudentManagementService;
using UniversityManagement.WPF.TeacherManagementService;

namespace UniversityManagement.WPF;

public partial class MainWindow
{
    private readonly UniversityDbContext _dbContext;
    private readonly GroupService _groupService;
    private readonly TeacherService _teacherService;
    private readonly StudentService _studentService;
    private readonly DocxService _docxService;
    private readonly PdfService _pdfService;
    private readonly CsvService _csvService;

    public MainWindow(UniversityDbContext dbContext)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _groupService = new GroupService(_dbContext);
        _teacherService = new TeacherService(_dbContext);
        _studentService = new StudentService(_dbContext);
        _docxService = new DocxService(_dbContext);
        _pdfService = new PdfService(_dbContext);
        _csvService = new CsvService(_dbContext);

        LoadEntitiesData();

        CourseListView.ItemsSource = _dbContext.Courses.ToList();

        CourseListView.SelectionChanged += CourseListView_SelectionChanged;
        
        GroupListView.SelectionChanged += GroupListView_SelectionChanged;
    }

    private void CourseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CourseListView.SelectedItem != null)
        {
            var selectedCourse = (Course)CourseListView.SelectedItem;

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

            StudentListView.ItemsSource = selectedGroup.Students.ToList();
        }
        else
        {
            StudentListView.ItemsSource = null;
        }
    }

    private void OpenGroupManagementWindow_Click(object sender, RoutedEventArgs e)
    {
        var groupManagementWindow = new GroupManagementWindow(_groupService, _docxService, _pdfService);
        groupManagementWindow.ShowDialog();
    }
    
    private void OpenStudentManagementWindow_Click(object sender, RoutedEventArgs e)
    {
        var studentManagementWindow = new StudentManagementWindow(_studentService, _csvService);
        studentManagementWindow.ShowDialog();
    }
    
    private void OpenTeacherManagementWindow_Click(object sender, RoutedEventArgs e)
    {
        var teacherManagementWindow = new TeacherManagementWindow(_teacherService);
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