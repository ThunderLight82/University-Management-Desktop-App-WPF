using System.Windows;
using System.Windows.Controls;
using UniversityManagement.Data;
using UniversityManagement.Entities;

namespace UniversityManagement.TeacherManagementService;

public partial class TeacherManagementWindowChangeTeacherDataPage
{
    private UniversityDbContext _dbContext;
    private TeacherService _teacherService;

    public TeacherManagementWindowChangeTeacherDataPage(UniversityDbContext dbContext, TeacherService teacherService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _teacherService = teacherService;

        TeachersListView.ItemsSource = _dbContext.Teachers.Local.ToObservableCollection();
    }

    private void ChangeTeacherNameAndWorkInfo_Click(object sender, RoutedEventArgs e)
    {
        var selectedTeacher = TeachersListView.SelectedItem as Teacher;

        string changedTeacherFullName = ChangeTeacherFullNameTextBox.Text.Trim();

        if (IsCorrespondence.SelectedItem is ComboBoxItem selectedItem)
        {
            bool isCorrespondence = selectedItem.Content.ToString() == "Yes";

            if (_teacherService.ChangeTeacherNameAndWorkInfo(selectedTeacher, changedTeacherFullName, isCorrespondence))
            {
                TeachersListView.Items.Refresh();
            }
        }
    }

    private void FillInfoBlockWithSelectedTeacherFromListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TeachersListView.SelectedItem is Teacher selectedTeacher)
        {
            ChangeTeacherFullNameTextBox.Text = selectedTeacher.TeacherFullName;

            IsCorrespondence.SelectedIndex = selectedTeacher.IsCorrespondence ? 0 : 1;
        }
    }
}