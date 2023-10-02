using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;

namespace DesktopApplication.Student_Management;

public partial class StudentManagementWindowExpOrImpStudentsToGroupPage
{
    private UniversityDbContext _dbContext;
    public StudentManagementWindowExpOrImpStudentsToGroupPage(UniversityDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
        CourseComboBox.ItemsSource = _dbContext.Courses;
        // Maybe use " CourseComboBox.ItemsSource = _dbContext.Courses.ToList()" instead???
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    private async void ExportStudents_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (selectedCourse != null && !string.IsNullOrEmpty(selectedGroupName))
        {
            var selectedGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);

            if (selectedGroup != null)
            {
                var studentToExport = _dbContext.Students.Where(student =>
                    !string.IsNullOrWhiteSpace(student.CurrentGroupName) &&
                    student.CurrentGroupName == selectedGroupName).ToList();

                if (studentToExport.Any())
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

                        await using var streamWriter = new StreamWriter(filePath);
                        await using var csv = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));
                        {
                            await csv.WriteRecordsAsync(studentToExport);
                        }

                        MessageBox.Show("Students have been successfully exported from selected group!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("No students found in the selected group", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("The selected group does not exist for the chosen course", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select both a course and a group to export students from", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    

    private void ImportStudents_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (selectedCourse != null && !string.IsNullOrEmpty(selectedGroupName))
        {
            var selectedGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);

            if (selectedGroup != null)
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "CVS Files (*.csv)|*.csv"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;

                    var userAnswerResult = MessageBox.Show("This action will overwrite the current group data. Do you want to continue?",
                        "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (userAnswerResult == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using var streamReader = new StreamReader(filePath);
                            using var csv = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture));

                            var studentsToImport =  csv.GetRecords<Student>().ToList();

                            selectedGroup.Students.Clear();

                            foreach (var importedStudent in studentsToImport)
                            {
                                importedStudent.CurrentGroupName = selectedGroupName;
                                selectedGroup.Students.Add(importedStudent);

                                var existingStudent = _dbContext.Students.FirstOrDefault(s =>
                                    s.StudentId == importedStudent.StudentId);

                                if (existingStudent != null)
                                {
                                    existingStudent.CurrentGroupName = selectedGroupName;
                                }
                                else
                                {
                                    _dbContext.Students.Add(importedStudent);
                                }
                            }

                            MessageBox.Show("Students have been successfully imported to the selected group!", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred while importing students list from group: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("No students found in the selected group", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("The selected group does not exist for the chosen course", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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