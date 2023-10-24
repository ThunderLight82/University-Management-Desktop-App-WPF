using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.StudentManagementService;

public partial class StudentManagementWindowExpOrImpStudentsToGroupPage
{
    private UniversityDbContext _dbContext;
    private StudentService _studentService;

    public StudentManagementWindowExpOrImpStudentsToGroupPage(UniversityDbContext dbContext,
        StudentService studentService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _studentService = studentService;

        CourseComboBox.ItemsSource = _dbContext.Courses.Local.ToObservableCollection();

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    private async void ExportStudents_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (selectedCourse != null && !string.IsNullOrEmpty(selectedGroupName))
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CVS Files (.csv)|*.csv",
                DefaultExt = ".csv",
                FileName = $"{selectedCourse.CourseName} {selectedGroupName} Students List"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                bool exportResult = await _studentService.ExportStudentsAsync(selectedCourse, selectedGroupName, filePath);

                if (exportResult)
                {
                    MessageBox.Show("Students have been successfully exported!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("An error occurred during export. Please, try again", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please, select both a course and a group to export students from", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void ImportStudents_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (selectedCourse != null && !string.IsNullOrEmpty(selectedGroupName))
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string importFilePath = openFileDialog.FileName;

                bool importResult = await _studentService.ImportStudentsAsync(selectedCourse, selectedGroupName, importFilePath);

                if (importResult)
                {
                    MessageBox.Show("Students have been successfully imported to the selected group!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to import students to the selected group. Please, try again", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please, select both a course and a group to import students to", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;

        GroupComboBox.ItemsSource = selectedCourse?.Groups.Select(group => group.GroupName).ToList();
    }
}