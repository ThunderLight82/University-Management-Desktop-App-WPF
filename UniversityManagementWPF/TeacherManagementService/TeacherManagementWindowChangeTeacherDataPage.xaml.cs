using System.Windows;
using System.Windows.Controls;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.TeacherManagementService;

public partial class TeacherManagementWindowChangeTeacherDataPage
{
    private TeacherService _teacherService;

    public TeacherManagementWindowChangeTeacherDataPage(TeacherService teacherService)
    {
        InitializeComponent();

        _teacherService = teacherService;

        TeachersListView.ItemsSource = _teacherService.PopulateTeacherList();
    }

    private void ChangeTeacherNameAndWorkInfo_Click(object sender, RoutedEventArgs e)
    {
        var selectedTeacher = TeachersListView.SelectedItem as Teacher;

        if (selectedTeacher == null)
        {
            MessageBox.Show("Please, select teacher from list first to update info", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return;
        }

        string changedTeacherFullName = ChangeTeacherFullNameTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(changedTeacherFullName))
        {
            MessageBox.Show("Please, enter a valid teacher name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return;
        }

        if (IsCorrespondence.SelectedItem is ComboBoxItem selectedItem)
        {
            bool isCorrespondence = selectedItem.Content.ToString() == "Yes";

            _teacherService.ChangeTeacherNameAndWorkInfo(selectedTeacher, changedTeacherFullName, isCorrespondence);
            
            TeachersListView.ItemsSource = _teacherService.PopulateTeacherList();
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