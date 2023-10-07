using System.Windows;
using System.Windows.Controls;
using DesktopApplication.Entities;

namespace DesktopApplication.StudentManagementService;

public partial class StudentManagementWindowChangeStudentDataPage
{
    private UniversityDbContext _dbContext;

    public StudentManagementWindowChangeStudentDataPage(UniversityDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
        StudentsListView.ItemsSource = _dbContext.Students;
        // Maybe use "StudentsListView.ItemsSource = _dbContext.Students.Local" instead???
    }

    private void ChangeStudentNameAndWorkInfo_Click (object sender, RoutedEventArgs e)
    {
        if (StudentsListView.SelectedItem is Student selectedStudent)
        {
            string changedStudentFullname = ChangeStudentFullNameTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(changedStudentFullname))
            {
                selectedStudent.StudentFullName = changedStudentFullname;

                var selectedComboBoxItem = IsWorkingComboBox.SelectedItem as ComboBoxItem;

                if (selectedComboBoxItem != null)
                {
                    var selectedItemContent = selectedComboBoxItem.Content.ToString();
                    selectedStudent.IsWorkingInDepartment = selectedItemContent == "Yes";
                }

                _dbContext.SaveChanges();

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