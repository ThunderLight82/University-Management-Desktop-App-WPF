using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;

namespace UniversityManagement.Services;

public class PdfService
{
    private readonly UniversityDbContext _dbContext;

    public PdfService(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateGroupInfoPdfFileAsync(Course selectedCourse, string selectedGroupName, string filePath)
    {
        try
        {
            if (selectedCourse == null && string.IsNullOrWhiteSpace(selectedGroupName))
                throw new ArgumentNullException();

            var selectedGroup = _dbContext.Groups
                .Include(group => group.Students)
                .FirstOrDefault(group => group.CourseId == selectedCourse!.CourseId && group.GroupName == selectedGroupName);

            var studentsToExport = await GetStudentsListWithinGroupAsync(selectedGroupName);

            if (studentsToExport.Any())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);

                var courseTitleFont = new XFont("Arial", 18, XFontStyle.Bold);
                var groupTitleFont = new XFont("Arial", 14, XFontStyle.Bold);
                var textFont = new XFont("Arial", 12, XFontStyle.Regular);

                gfx.DrawString(selectedCourse!.CourseName, courseTitleFont, XBrushes.Black, new XRect(0, 40, page.Width, 0), XStringFormats.TopCenter);
                gfx.DrawString(selectedGroup!.GroupName, groupTitleFont, XBrushes.Black, new XRect(0, 70, page.Width, 0), XStringFormats.TopCenter);

                var students = selectedGroup.Students.Select(student => student.StudentFullName).ToList();

                for (int i = 0; i < students.Count; i++)
                {
                    byte[] utf8BytesConvert = Encoding.UTF8.GetBytes($"{i + 1}. {students[i]}");
                    string utf8Text = Encoding.UTF8.GetString(utf8BytesConvert);

                    gfx.DrawString(utf8Text, textFont, XBrushes.Black, new XRect(50, 100 + i * 20, page.Width - 100, 0));
                }

                document.Save(filePath);
            }
            else
            {
                throw new InvalidOperationException();
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<Student>> GetStudentsListWithinGroupAsync(string selectedGroupName)
    {
        return await _dbContext.Students.Where(student =>
            !string.IsNullOrWhiteSpace(student.CurrentGroupName) &&
            student.CurrentGroupName == selectedGroupName).ToListAsync();
    }
}