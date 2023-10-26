using Microsoft.EntityFrameworkCore;
using System.Text;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;
using Xunit;

namespace UniversityManagement.UnitTests;

public class CsvServiceTests
{
    private readonly UniversityDbContext _dbContext;
    private readonly CsvService _csvService;
    private readonly Course _selectedCourse;

    public CsvServiceTests()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var options = new DbContextOptionsBuilder<UniversityDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDBUniversityDatabase")
            .Options;

        _dbContext = new UniversityDbContext(options);
        _csvService = new CsvService(_dbContext);

        _selectedCourse = new Course
        {
            CourseName = "TestCourse1",
        };

        _dbContext.Courses.Add(_selectedCourse);
        _dbContext.SaveChanges();
    }

    [Theory]
    [InlineData("GroupWithSomeStudent", true, "StudentWithinGroupTest,False,GroupWithSomeStudent,")]
    [InlineData("NonExistedGroup", false, null)]
    [InlineData("EmptyGroup", false, null)]
    public async Task ExportStudentsAsync_DifferentExportScenarios_ReturnsExpectedResult(
        string selectedGroupName,
        bool expectedExportResult,
        string expectedCsvContent)
    {
        // Arrange
        FillStudentsTestsWithAbstractData(_selectedCourse);
        string currentDirectory = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(currentDirectory, "test.csv");
    
        // Act
        var exportResult = await _csvService.ExportStudentsAsync(_selectedCourse, selectedGroupName, filePath);
    
        // Assert
        Assert.Equal(expectedExportResult, exportResult);
    
        if (expectedExportResult)
        {
            Assert.True(File.Exists(filePath));
    
            using var reader = new StreamReader(filePath);
            var csvFileWithContent = await reader.ReadToEndAsync();
    
            Assert.Contains("StudentId,StudentFullName,IsWorkingInDepartment,CurrentGroupName,GroupId", csvFileWithContent);
            if (expectedCsvContent != null)
            {
                Assert.Contains(expectedCsvContent, csvFileWithContent);
            }
        }
    }
    
    [Theory]
    [InlineData("GroupWithSomeStudent", true,
        "StudentWithinGroupTest,False,GroupWithSomeStudent,")]
    // Different Non existing student tests. Adding into db and selected group.
    [InlineData("EmptyGroup", true, "StudentFullName,IsWorkingInDepartment,CurrentGroupName\r\nNonExistedInDBStudent,False,EmptyGegegroup,3f3g")]
    [InlineData("EmptyGroup", true, "StudentFullName,IsWorkingInDepartment,CurrentGroupName\r\nNonExistedInDBStudent,False,EmptyGroup,")]
    [InlineData("NonExistedGroup", false, null)]
    public async Task ImportStudentsAsync_ImportScenarios_ReturnsExpectedResult(
        string selectedGroupName,
        bool expectedImportResult,
        string expectedCsvContent)
    {
        //Arrange
        FillStudentsTestsWithAbstractData(_selectedCourse);
        string currentDirectory = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(currentDirectory, "test.csv");
    
        if (!string.IsNullOrWhiteSpace(expectedCsvContent))
        {
            await File.WriteAllTextAsync(filePath, expectedCsvContent);
        }
    
        //Act 
        var importResult = await _csvService.ImportStudentsAsync(_selectedCourse, selectedGroupName, filePath);
    
        //Assert
        Assert.Equal(expectedImportResult, importResult);
    }

    private void FillStudentsTestsWithAbstractData(Course selectedCourse)
    {
        var students = new List<Student>
        {
            // case for [CsvService] tests.
            new() { StudentFullName = "StudentWithinGroupTest", CurrentGroupName = "GroupWithSomeStudent"}
        };

        _dbContext.Students.AddRange(students);
        _dbContext.SaveChanges();

        var groups = new List<Group>
        {
            // cases for [CsvService] tests.
            new() { GroupName = "EmptyGroup", CourseId = selectedCourse.CourseId},
            new() { GroupName = "GroupWithSomeStudent", CourseId = selectedCourse.CourseId },
        };

        _dbContext.Groups.AddRange(groups);
        _dbContext.SaveChanges();
    }
}