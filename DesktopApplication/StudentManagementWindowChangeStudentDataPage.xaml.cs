using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class StudentManagementWindowChangeStudentDataPage : Page
{
    private DataRepository _dataRepository;

    public StudentManagementWindowChangeStudentDataPage(DataRepository dataRepository)
    { 
        InitializeComponent();
        _dataRepository = dataRepository;
        StudentsListView.ItemsSource = _dataRepository.Students;
    }

    private void ChangeStudentNameAndWorkInfo_Click (object sender, RoutedEventArgs e)
    {
        if (StudentsListView.SelectedItem is Student selectedStudent)
        {
            string changedStudentFullname = ChangeStudentFullNameTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(changedStudentFullname))
            {
                selectedStudent.StudentFullName = changedStudentFullname;

                ComboBoxItem selectedComboBoxItem = IsWorkingComboBox.SelectedItem as ComboBoxItem;

                if (selectedComboBoxItem != null)
                {
                    string selectedItemContent = selectedComboBoxItem.Content.ToString();
                    selectedStudent.IsWorkingInDepartment = selectedItemContent == "Yes";
                }

                StudentsListView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please, enter a valid student name", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select student from list first to update info", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void FillInfoBlockWithSelectedStudentFromListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (StudentsListView.SelectedItem is Student selectedStudent)
        {
            ChangeStudentFullNameTextBox.Text = selectedStudent.StudentFullName;

            IsWorkingComboBox.SelectedIndex = selectedStudent.IsWorkingInDepartment ? 0 : 1;
        }
    }
}