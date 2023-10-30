using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.StudentManagementService;

public partial class StudentManagementWindowExpOrImpStudentsToGroupPage
{
    private StudentService _studentService;
    private CsvService _csvService;

    public StudentManagementWindowExpOrImpStudentsToGroupPage(StudentService studentService, CsvService csvService)
    {
        InitializeComponent();

        _csvService = csvService;
        _studentService = studentService;

        CourseComboBox.ItemsSource = _studentService.PopulateCourseList();

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    private async void ExportStudentsAsync_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (selectedCourse != null && !string.IsNullOrWhiteSpace(selectedGroupName))
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CVS Files (.csv)|*.csv",
                DefaultExt = ".csv",
                FileName = $"{selectedCourse.CourseName} {selectedGroupName} Students List"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var studentsToExport = await _csvService.GetStudentsToExportAsync(selectedGroupName);

                if (studentsToExport.Any())
                {
                    string exportFilePath = saveFileDialog.FileName;

                    bool exportResult = await _csvService.ExportStudentsAsync(selectedCourse, selectedGroupName, exportFilePath);

                    if (exportResult)
                    {
                        MessageBox.Show("Students have been successfully exported!", 
                            "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("An error occurred during export. Please, try again", 
                            "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No students found in the selected group", 
                        "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please, select both a course and a group to export students from", 
                "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void ImportStudentsAsync_Click(object sender, RoutedEventArgs e)
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
                var userAnswerResult = MessageBox.Show(
                    "This action will overwrite the current group student list. Do you want to continue?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (userAnswerResult == MessageBoxResult.Yes)
                {
                    string importFilePath = openFileDialog.FileName;

                    bool importResult = await _csvService.ImportStudentsAsync(selectedCourse, selectedGroupName, importFilePath);

                    if (importResult)
                    {
                        MessageBox.Show("Students have been successfully imported to the selected group!", 
                            "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to import students to the selected group. Please, try again", 
                            "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        else
        {
            MessageBox.Show("Please, select both a course and a group to import students to", 
                "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;

        GroupComboBox.ItemsSource = selectedCourse?.Groups.Select(group => group.GroupName).ToList();
    }
}