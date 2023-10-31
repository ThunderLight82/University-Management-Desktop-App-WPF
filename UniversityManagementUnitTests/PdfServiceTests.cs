using Microsoft.EntityFrameworkCore;
using System.Text;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;
using Xunit;

namespace UniversityManagement.UnitTests;

public class PdfServiceTests
{
    private readonly UniversityDbContext _dbContext;
    private readonly PdfService _pdfService;
    private readonly Course _selectedCourse;

    public PdfServiceTests()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var options = new DbContextOptionsBuilder<UniversityDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDBUniversityDatabase")
            .Options;

        _dbContext = new UniversityDbContext(options);
        _pdfService = new PdfService(_dbContext);

        _selectedCourse = new Course
        {
            CourseName = "TestCourse4"
        };

        _dbContext.Courses.Add(_selectedCourse);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task CreateGroupInfoPdfFileAsync_GroupWithStudents_CreatesPdfFileAndCheckStringsInFile()
    {
        // Arrange
        await FillGroupsTestsWithAbstractDataAsync(_selectedCourse);
    
        //Act
        var pdfCreationResult =
            await _pdfService.CreateGroupInfoPdfFileAsync(_selectedCourse, "GroupWithStudentsInIt", "test.pdf");
    
        // Assert
        Assert.True(pdfCreationResult);
        Assert.True(File.Exists("test.pdf"));
        Assert.True(new FileInfo("test.pdf").Length > 0);
    }
    
    [Fact]
    public async Task CreateGroupInfoPdfFileAsync_EmptyGroup_GetFalseResult()
    {
        // Arrange
        await FillGroupsTestsWithAbstractDataAsync(_selectedCourse);
    
        // Act
        var pdfCreationResult = await _pdfService.CreateGroupInfoPdfFileAsync(_selectedCourse, "EmptyGroupY", "test.pdf");
    
        // Assert
        Assert.False(pdfCreationResult);
    }

    private async Task FillGroupsTestsWithAbstractDataAsync(Course selectedCourse)
    {
        var groups = new List<Group>
        {
            // case for [CreateGroupInfoPdfFile_GroupWithStudents] test.
            new() { GroupName = "GroupWithStudentsInIt", CourseId = selectedCourse.CourseId },
            // case for [CreateGroupInfoPdfFile_EmptyGroup] test.
            new() { GroupName = "EmptyGroupY", CourseId = selectedCourse.CourseId }
        };

        _dbContext.Groups.AddRange(groups);

        // Insert existing students into group to get "true" in [CreateGroupInfoPdfFile_GroupWithStudents]
        var newStudentInGroup = new Student
        {
            StudentFullName = "TestStudent",
            CurrentGroupName = "GroupWithStudentsInIt",
            GroupId = groups.First(g => g.GroupName == "GroupWithStudentsInIt").GroupId
        };

        _dbContext.Students.Add(newStudentInGroup);
        await _dbContext.SaveChangesAsync();
    }
}