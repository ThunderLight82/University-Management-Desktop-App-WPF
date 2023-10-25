using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.GroupManagementService;

public partial class GroupManagementWindowCreateFileWithGroupInfoPage
{
    private UniversityDbContext _dbContext;
    private DocxService _docxService;
    private PdfService _pdfService;

    public GroupManagementWindowCreateFileWithGroupInfoPage(UniversityDbContext dbContext, PdfService pdfService, DocxService docxService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _pdfService = pdfService;
        _docxService = docxService;

        CourseComboBox.ItemsSource = _dbContext.Courses.Local.ToObservableCollection();
    }

    private void CreateGroupInfoDocxFile_Click(object sender, RoutedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (selectedCourse != null && !string.IsNullOrEmpty(selectedGroupName))
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Documents (.docx)|*.docx",
                DefaultExt = ".docx",
                FileName = $"{selectedCourse.CourseName} {selectedGroupName} Students List"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                bool exportResult = _docxService.CreateGroupInfoDocxFile(selectedCourse, selectedGroupName, filePath);

                if (exportResult)
                {
                    MessageBox.Show("(.docx) file with selected group successfully created!", "Success",
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
            MessageBox.Show("Please, select both a course and a group to save students list from", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CreateGroupInfoPdfFile_Click(object sender, RoutedEventArgs e)
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
                string filePath = saveFileDialog.FileName;

                bool exportResult = _pdfService.CreateGroupInfoPdfFile(selectedCourse, selectedGroupName, filePath);

                if (exportResult)
                {
                    MessageBox.Show("(.pdf) file with selected group successfully created!", "Success",
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
            MessageBox.Show("Please, select both a course and a group to save students list from", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedCourse = CourseComboBox.SelectedItem as Course;

        GroupComboBox.ItemsSource = selectedCourse?.Groups.Select(group => group.GroupName).ToList();
    }
}