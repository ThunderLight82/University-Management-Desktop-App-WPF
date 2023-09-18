using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class StudentManagementWindowChangeStudentData : Page
{
    private DataRepository _dataRepository;

    public StudentManagementWindowChangeStudentData(DataRepository dataRepository)
    { 
        InitializeComponent();
        _dataRepository = dataRepository;
        StudentsListView.ItemsSource = _dataRepository.Students;
    }

    private void ChangeStudentNameAndWorkInfo_Click (object sender, RoutedEventArgs e)
    {
        string changedStudentFullname = ChangeStudentFullNameTextBox.Text.Trim();

        if (StudentsListView.SelectedItem is Student selectedStudent)
        {
            selectedStudent.StudentFullName = changedStudentFullname;
            // [WiP]
        }
        else
        {
            MessageBox.Show("Please, select student from list first", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // private void EditGroupName_Click(object sender, RoutedEventArgs e)
    // {
    //     string selectedGroupName = EditGroupNameComboBox.SelectedItem as string;
    //     string newGroupName = EditGroupNameTextBox.Text.Trim();
    //
    //     Course selectedCourse = CourseComboBox.SelectedItem as Course;
    //
    //     if (selectedCourse == null)
    //     {
    //         MessageBox.Show("Please, select a course first to edit the group name within", "Error",
    //             MessageBoxButton.OK, MessageBoxImage.Error);
    //         return;
    //     }
    //
    //     if (string.IsNullOrWhiteSpace(selectedGroupName) || string.IsNullOrWhiteSpace(newGroupName))
    //     {
    //         MessageBox.Show("Please, select a group from list and provide a new group name", "Error",
    //             MessageBoxButton.OK, MessageBoxImage.Error);
    //         return;
    //     }
    //
    //     Group editGroupName = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);
    //
    //     editGroupName.GroupName = newGroupName;
    //
    //     EditGroupNameTextBox.Clear();
    //
    //     ComboBoxRefreshAll(null, null);
    // }

    // private void AddStudent_Click(object sender, RoutedEventArgs e)
    // {
    //     string newStudentFullName = NewStudentFullNameTextBox.Text.Trim();
    //
    //     if (!string.IsNullOrWhiteSpace(newStudentFullName))
    //     {
    //         bool newStudentNameAlreadyExists = _dataRepository.Students.Any(student =>
    //             student.StudentFullName.Equals(newStudentFullName, StringComparison.OrdinalIgnoreCase));
    //
    //         if (newStudentNameAlreadyExists)
    //         {
    //             var duplicateStudentQuestion = MessageBox.Show(
    //                 "A student with the same name already exists. Do you want to add this student anyway?",
    //                 "Duplication name",
    //                 MessageBoxButton.YesNo,
    //                 MessageBoxImage.Question);
    //
    //             if (duplicateStudentQuestion == MessageBoxResult.No)
    //             {
    //                 return;
    //             }
    //         }
    //
    //         int lastNewestStudentId = _dataRepository.Students.Max(student => student.StudentId);
    //
    //         var newStudent = new Student()
    //         {
    //             StudentId = lastNewestStudentId + 1,
    //             StudentFullName = newStudentFullName,
    //             IsWorkingInDepartment = false
    //         };
    //
    //         _dataRepository.Students.Add(newStudent);
    //
    //         NewStudentFullNameTextBox.Clear();
    //
    //         StudentsListView.Items.Refresh();
    //     }
    //     else
    //     {
    //         MessageBox.Show("Please, enter a valid student name", "Error",
    //             MessageBoxButton.OK, MessageBoxImage.Error);
    //     }
    // }
}