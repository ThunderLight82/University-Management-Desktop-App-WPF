using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DesktopApplication.Entities;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace DesktopApplication.GroupManagementService;

public partial class GroupManagementWindowCreateFileWithGroupInfoPage
{
    private UniversityDbContext _dbContext;

    public GroupManagementWindowCreateFileWithGroupInfoPage(UniversityDbContext dbContext)
    {
        InitializeComponent();

        _dbContext = dbContext;

        CourseComboBox.ItemsSource = _dbContext.Courses.Local.ToObservableCollection();
    }

    private void CreateGroupInfoDocxFile_Click(object sender, RoutedEventArgs e)
    {
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (CourseComboBox.SelectedItem is Course selectedCourse && !string.IsNullOrEmpty(selectedGroupName))
        {
            var selectedGroup = _dbContext.Groups
                .Include(group => group.Students)
                .FirstOrDefault(group => group.CourseId == selectedCourse.CourseId && group.GroupName == selectedGroupName);

            if (selectedGroup != null)
            {
                if (!selectedGroup.Students.Any())
                {
                    MessageBox.Show("The selected group is empty. There are no students to include in the document", "Empty Group",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var saveNewFileDialog = new SaveFileDialog
                {
                    Filter = "Word Documents (.docx)|*.docx",
                    DefaultExt = ".docx",
                    FileName = $"{selectedCourse.CourseName} {selectedGroupName} Students List"
                };

                if (saveNewFileDialog.ShowDialog() == true)
                {
                    string filePath = saveNewFileDialog.FileName;

                    using var doc = DocX.Create(filePath);

                    doc.InsertParagraph(selectedCourse.CourseName).Bold().FontSize(18).Alignment = Alignment.center;
                    doc.InsertParagraph(selectedGroup.GroupName).Bold().FontSize(14).Alignment = Alignment.center;

                    var students = selectedGroup.Students.Select(student => student.StudentFullName).ToList();

                    for (int i = 0; i < students.Count; i++)
                    {
                        doc.InsertParagraph($"{i + 1}. {students[i]}");
                    }

                    doc.Save();

                    MessageBox.Show("(.docx) file with selected group successfully generated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
            MessageBox.Show("Please, select both a course and a group to save students list from", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CreateGroupInfoPdfFile_Click(object sender, RoutedEventArgs e)
    {
        var selectedGroupName = GroupComboBox.SelectedItem as string;

        if (CourseComboBox.SelectedItem is Course selectedCourse && !string.IsNullOrEmpty(selectedGroupName))
        {
            var selectedGroup = _dbContext.Groups
                .Include(group => group.Students)
                .FirstOrDefault(group => group.CourseId == selectedCourse.CourseId && group.GroupName == selectedGroupName);

            if (selectedGroup != null)
            {
                if (!selectedGroup.Students.Any())
                {
                    MessageBox.Show("The selected group is empty. There are no students to include in the document", "Empty Group",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var saveNewFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (.pdf)|*.pdf",
                    DefaultExt = ".pdf",
                    FileName = $"{selectedCourse.CourseName} {selectedGroupName} Students List"
                };

                if (saveNewFileDialog.ShowDialog() == true)
                {
                    string filePath = saveNewFileDialog.FileName;

                    var document = new PdfDocument();
                    var page = document.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);

                    var courseTitleFont = new XFont("Arial", 18, XFontStyle.Bold);
                    var groupTitleFont = new XFont("Arial", 14, XFontStyle.Bold);
                    var textFont = new XFont("Arial", 12, XFontStyle.Regular);

                    gfx.DrawString(selectedCourse.CourseName, courseTitleFont, XBrushes.Black, new XRect(0, 40, page.Width, 0), XStringFormats.TopCenter);
                    gfx.DrawString(selectedGroup.GroupName, groupTitleFont, XBrushes.Black, new XRect(0, 70, page.Width, 0), XStringFormats.TopCenter);

                    var students = selectedGroup.Students.Select(student => student.StudentFullName).ToList();

                    for (int i = 0; i < students.Count; i++)
                    {
                        byte[] utf8BytesConvert = Encoding.UTF8.GetBytes($"{i + 1}. {students[i]}");
                        string utf8Text = Encoding.UTF8.GetString(utf8BytesConvert);

                        gfx.DrawString(utf8Text, textFont, XBrushes.Black, new XRect(50, 100 + i * 20, page.Width - 100, 0));
                    }

                    document.Save(filePath);

                    MessageBox.Show("(.pdf) file with selected group successfully generated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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