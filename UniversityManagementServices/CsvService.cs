using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;

namespace UniversityManagement.Services;

public class CsvService
{
    private UniversityDbContext _dbContext;

    public CsvService(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExportStudentsAsync(Course selectedCourse, string selectedGroupName, string filePath)
    {
        var selectedGroup = selectedCourse.Groups.FirstOrDefault(group =>
            group.GroupName == selectedGroupName);

        if (selectedGroup == null)
        {
            MessageBox.Show("The selected group does not exist for the chosen course", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        var studentsToExport = _dbContext.Students.Where(student =>
            !string.IsNullOrWhiteSpace(student.CurrentGroupName) &&
            student.CurrentGroupName == selectedGroupName).ToList();

        if (!studentsToExport.Any())
        {
            MessageBox.Show("No students found in the selected group", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        await using var streamWriter = new StreamWriter(filePath);
        await using var csv = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));
        {
            await csv.WriteRecordsAsync(studentsToExport);
        }

        return true;
    }

    public async Task<bool> ImportStudentsAsync(Course selectedCourse, string selectedGroupName, string filePath)
    {
        var selectedGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == selectedGroupName);

        if (selectedGroup == null)
        {
            MessageBox.Show("The selected group does not exist for the chosen course", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        var userAnswerResult = MessageBox.Show(
            "This action will overwrite the current group student list. Do you want to continue?",
            "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (userAnswerResult == MessageBoxResult.Yes)
        {
            using var streamReader = new StreamReader(filePath);
            using var csv = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            var studentsToRemove = selectedGroup.Students.ToList();

            foreach (var student in studentsToRemove)
            {
                student.GroupId = null;
                student.CurrentGroupName = null;
            }
            await _dbContext.SaveChangesAsync();

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

            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }
}