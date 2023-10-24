using System.Windows;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.TeacherManagementService;

public partial class TeacherManagementWindowEditTeacherPage
{
    private UniversityDbContext _dbContext;
    private TeacherService _teacherService;

    public TeacherManagementWindowEditTeacherPage(UniversityDbContext dbContext, TeacherService teacherService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _teacherService = teacherService;

        TeachersListView.ItemsSource = _dbContext.Teachers.Local.ToObservableCollection();
    }

    private void CreateTeacher_Click(object sender, RoutedEventArgs e)
    {
        string newTeacherFullName = NewTeacherFullNameTextBox.Text.Trim();

        if (_teacherService.CreateTeacher(newTeacherFullName))
        {
            NewTeacherFullNameTextBox.Clear();

            TeachersListView.Items.Refresh();
        }
    }

    private void DeleteTeacher_Click(object sender, RoutedEventArgs e)
    {
        var selectedTeacher = TeachersListView.SelectedItem as Teacher;

        if (_teacherService.DeleteTeacher(selectedTeacher))
        {
            TeachersListView.Items.Refresh();
        }
    }
}