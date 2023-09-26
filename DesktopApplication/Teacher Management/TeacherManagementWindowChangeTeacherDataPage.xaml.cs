using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication.Teacher_Management;

public partial class TeacherManagementWindowChangeTeacherDataPage
{
    private DataRepository _dataRepository;
    public TeacherManagementWindowChangeTeacherDataPage(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
        TeachersListView.ItemsSource = _dataRepository.Teachers;
    }

    private void ChangeTeacherNameAndWorkInfo_Click(object sender, RoutedEventArgs e)
    {
        if (TeachersListView.SelectedItem is Teacher selectedTeacher)
        {
            string changedTeacherFullname = ChangeTeacherFullNameTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(changedTeacherFullname))
            {
                selectedTeacher.TeacherFullName = changedTeacherFullname;

                var selectedComboBoxItem = IsCorrespondence.SelectedItem as ComboBoxItem;

                if (selectedComboBoxItem != null)
                {
                    var selectedItemContent = selectedComboBoxItem.Content.ToString();
                    selectedTeacher.IsCorrespondence = selectedItemContent == "Yes";
                }

                TeachersListView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please, enter a valid teacher name", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select teacher from list first to update info", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
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