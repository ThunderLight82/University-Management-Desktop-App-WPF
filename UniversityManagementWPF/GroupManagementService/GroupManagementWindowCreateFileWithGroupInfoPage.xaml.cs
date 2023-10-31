using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.GroupManagementService;

public partial class GroupManagementWindowCreateFileWithGroupInfoPage
{
    private GroupService _groupService;
    private DocxService _docxService;
    private PdfService _pdfService;

    public GroupManagementWindowCreateFileWithGroupInfoPage(GroupService groupService, DocxService docxService, PdfService pdfService)
    {
        InitializeComponent();

        _pdfService = pdfService;
        _docxService = docxService;
        _groupService = groupService;

        CourseComboBox.ItemsSource = _groupService.PopulateCourseList();
    }

    private async void CreateGroupInfoDocxFileAsync_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (selectedCourse != null && !string.IsNullOrWhiteSpace(selectedGroupName))
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Documents (.docx)|*.docx",
                DefaultExt = ".docx",
                FileName = $"{selectedCourse.CourseName} {selectedGroupName} Students List"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var studentsToExport = await _docxService.GetStudentsListWithinGroupAsync(selectedGroupName);

                if (studentsToExport.Any())
                {
                    string exportFilePath = saveFileDialog.FileName;

                    bool exportResult = await _docxService.CreateGroupInfoDocxFileAsync(selectedCourse, selectedGroupName, exportFilePath);

                    if (exportResult)
                    {
                        MessageBox.Show("(.docx) file with selected group successfully created!", 
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
                    MessageBox.Show("The selected group is empty. There are no students to include in the document",
                        "Empty Group",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please, select both a course and a group to save students list from", 
                "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void CreateGroupInfoPdfFileAsync_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (selectedCourse != null && !string.IsNullOrEmpty(selectedGroupName))
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (.pdf)|*.pdf",
                DefaultExt = ".pdf",
                FileName = $"{selectedCourse.CourseName} {selectedGroupName} Students List"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var studentsToExport = await _pdfService.GetStudentsListWithinGroupAsync(selectedGroupName);

                if (studentsToExport.Any())
                {

                    string exportFilePath = saveFileDialog.FileName;

                    bool exportResult = await _pdfService.CreateGroupInfoPdfFileAsync(selectedCourse, selectedGroupName, exportFilePath);

                    if (exportResult)
                    {
                        MessageBox.Show("(.pdf) file with selected group successfully created!", 
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
                    MessageBox.Show("The selected group is empty. There are no students to include in the document",
                        "Empty Group",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Please, select both a course and a group to save students list from", 
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