using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace UniversityManagement.Services;

public class DocxService
{
    private UniversityDbContext _dbContext;

    public DocxService(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool CreateGroupInfoDocxFile(Course selectedCourse, string selectedGroupName, string filePath)
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

                return false;
            }

            using var doc = DocX.Create(filePath);

            doc.InsertParagraph(selectedCourse.CourseName).Bold().FontSize(18).Alignment = Alignment.center;
            doc.InsertParagraph(selectedGroup.GroupName).Bold().FontSize(14).Alignment = Alignment.center;

            var students = selectedGroup.Students.Select(student => student.StudentFullName).ToList();

            for (int i = 0; i < students.Count; i++)
            {
                doc.InsertParagraph($"{i + 1}. {students[i]}");
            }

            doc.Save();
        }

        return true;
    }
}