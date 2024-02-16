using System;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UniversityManagement.Services;

public class CsvService
{
    private readonly UniversityDbContext _dbContext;

    public CsvService(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExportStudentsAsync(Course selectedCourse, string selectedGroupName, string exportFilePath)
    {
        try
        {
            if (selectedCourse == null && string.IsNullOrWhiteSpace(selectedGroupName))
                throw new ArgumentNullException();

            var studentsToExport = await GetStudentsToExportAsync(selectedGroupName);

            if (studentsToExport.Any())
            {
                await using var streamWriter = new StreamWriter(exportFilePath);
                await using var csv = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));
                await csv.WriteRecordsAsync(studentsToExport);
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

    public async Task<bool> ImportStudentsAsync(Course selectedCourse, string selectedGroupName, string importFilePath)
    {
        try
        {
            if (selectedCourse == null && string.IsNullOrWhiteSpace(selectedGroupName))
                throw new ArgumentNullException();

            using var streamReader = new StreamReader(importFilePath);
            using var csv = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            var selectedGroup = selectedCourse!.Groups.FirstOrDefault(group =>
                group.GroupName == selectedGroupName);

            var studentsToRemove = selectedGroup!.Students.ToList();

            foreach (var student in studentsToRemove)
            {
                student.GroupId = null;
                student.CurrentGroupName = null;
            }

            var studentsToImport = csv.GetRecords<Student>().ToList();

            foreach (var importedStudent in studentsToImport)
            {
                var existingStudent = _dbContext.Students.FirstOrDefault(s =>
                    s.StudentId == importedStudent.StudentId);

                if (existingStudent != null)
                {
                    existingStudent.CurrentGroupName = selectedGroup.GroupName;
                    existingStudent.GroupId = selectedGroup.GroupId;
                }
                else
                {
                    _dbContext.Students.Add(importedStudent);

                    importedStudent.CurrentGroupName = selectedGroup.GroupName;
                    importedStudent.GroupId = selectedGroup.GroupId;
                }

                await _dbContext.SaveChangesAsync();
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<Student>> GetStudentsToExportAsync(string selectedGroupName)
    {
        return await _dbContext.Students.Where(student =>
            !string.IsNullOrWhiteSpace(student.CurrentGroupName) &&
            student.CurrentGroupName == selectedGroupName).ToListAsync();
    }
}