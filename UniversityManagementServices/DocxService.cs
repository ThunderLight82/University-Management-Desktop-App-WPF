using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
        try
        {
            if (selectedCourse == null && string.IsNullOrWhiteSpace(selectedGroupName))
                throw new ArgumentNullException();

            var selectedGroup = _dbContext.Groups
                .Include(group => group.Students)
                .FirstOrDefault(group => group.CourseId == selectedCourse!.CourseId && group.GroupName == selectedGroupName);

            var studentsToExport = GetStudentsListWithinGroup(selectedGroupName);

            if (studentsToExport.Any())
            {
                using var doc = DocX.Create(filePath);

                doc.InsertParagraph(selectedCourse!.CourseName).Bold().FontSize(18).Alignment = Alignment.center;
                doc.InsertParagraph(selectedGroup!.GroupName).Bold().FontSize(14).Alignment = Alignment.center;

                var students = selectedGroup.Students.Select(student => student.StudentFullName).ToList();

                for (int i = 0; i < students.Count; i++)
                {
                    doc.InsertParagraph($"{i + 1}. {students[i]}");
                }

                doc.Save();
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

    public List<Student> GetStudentsListWithinGroup(string selectedGroupName)
    {
        return _dbContext.Students.Where(student =>
            !string.IsNullOrWhiteSpace(student.CurrentGroupName) &&
            student.CurrentGroupName == selectedGroupName).ToList();
    }
}