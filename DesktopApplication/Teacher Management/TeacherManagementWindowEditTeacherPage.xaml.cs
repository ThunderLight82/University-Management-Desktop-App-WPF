using System;
using System.Linq;
using System.Windows;

namespace DesktopApplication.Teacher_Management;

public partial class TeacherManagementWindowEditTeacherPage
{
    private DataRepository _dataRepository;
    public TeacherManagementWindowEditTeacherPage(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
        TeachersListView.ItemsSource = _dataRepository.Teachers;
    }

    private void AddTeacher_Click(object sender, RoutedEventArgs e)
    {
        string newTeacherFullName = NewTeacherFullNameTextBox.Text.Trim();

        if (!string.IsNullOrWhiteSpace(newTeacherFullName))
        {
            var newTeacherNameAlreadyExists = _dataRepository.Teachers.Any(teacher =>
                teacher.TeacherFullName.Equals(newTeacherFullName, StringComparison.OrdinalIgnoreCase));

            if (newTeacherNameAlreadyExists)
            {
                var duplicateTeacherQuestion = MessageBox.Show(
                    "A teacher with the same name already exists. Do you want to add this teacher anyway?",
                    "Duplication name",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (duplicateTeacherQuestion == MessageBoxResult.No)
                {
                    return;
                }
            }

            int lastNewestTeacherId = _dataRepository.Teachers.Max(teacher => teacher.TeacherId);

            var newTeacher = new Teacher
            {
                TeacherId = lastNewestTeacherId + 1,
                TeacherFullName = newTeacherFullName,
                IsCorrespondence = false
            };

            _dataRepository.Teachers.Add(newTeacher);

            NewTeacherFullNameTextBox.Clear();

            TeachersListView.Items.Refresh();
        }
        else
        {
            MessageBox.Show("Please, enter a valid teacher name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DeleteTeacher_Click(object sender, RoutedEventArgs e)
    {
        var selectedTeacher = TeachersListView.SelectedItem as Teacher;

        if (selectedTeacher != null)
        {
            var associatedGroup = _dataRepository.Groups.FirstOrDefault(group => group.GroupCurator.Contains(selectedTeacher));

            if (associatedGroup != null)
            {
                associatedGroup.GroupCurator.Remove(selectedTeacher);
            }

            selectedTeacher.CurrentGroupCurationName = null;

            _dataRepository.Teachers.Remove(selectedTeacher);

            TeachersListView.Items.Refresh();
        }
        else
        {
            MessageBox.Show("Please, select a teacher from the list below to remove", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}