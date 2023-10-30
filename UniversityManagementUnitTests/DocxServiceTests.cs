using Microsoft.EntityFrameworkCore;
using System.Text;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;
using Xceed.Words.NET;
using Xunit;

namespace UniversityManagement.UnitTests;

public class DocxServiceTests
{
    private readonly UniversityDbContext _dbContext;
    private readonly DocxService _docxService;
    private readonly Course _selectedCourse;

    public DocxServiceTests()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var options = new DbContextOptionsBuilder<UniversityDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDBUniversityDatabase")
            .Options;

        _dbContext = new UniversityDbContext(options);
        _docxService = new DocxService(_dbContext);

        _selectedCourse = new Course
        {
            CourseName = "TestCourse2"
        };

        _dbContext.Courses.Add(_selectedCourse);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task CreateGroupInfoDocxFileAsync_GroupWithStudents_CreatesDocxFileAndCheckStringsInFile()
    {
        // Arrange
        await FillGroupsTestsWithAbstractDataAsync(_selectedCourse);

        // Act
        var docxCreationResult =
            await _docxService.CreateGroupInfoDocxFileAsync(_selectedCourse, "GroupWithStudentsInIt", "test.docx");

        using var doc = DocX.Load("test.docx");
        var text = doc.Text;

        // Assert
        Assert.True(docxCreationResult);
        Assert.Contains("TestCourse2", text);
        Assert.Contains("GroupWithStudentsInIt", text);
        Assert.Contains("TestStudent22", text);

        File.Delete("test.docx");
    }

    [Fact]
    public async Task CreateGroupInfoDocxFileAsync_EmptyGroup_GetFalseResult()
    {
        // Arrange
        await FillGroupsTestsWithAbstractDataAsync(_selectedCourse);

        // Act
        var docxCreationResult = await _docxService.CreateGroupInfoDocxFileAsync(_selectedCourse, "1EmptyGroup", "test.docx");

        // Assert
        Assert.False(docxCreationResult);

        File.Delete("test.docx");
    }

    private async Task FillGroupsTestsWithAbstractDataAsync(Course selectedCourse)
    {
        var groups = new List<Group>
        {
            // case for [CreateGroupInfoDocxFile_GroupWithStudents] test.
            new() { GroupName = "GroupWithStudentsInIt", CourseId = selectedCourse.CourseId },
            // case for [CreateGroupInfoDocxFile_EmptyGroup] test.
            new() { GroupName = "1EmptyGroup", CourseId = selectedCourse.CourseId }
        };

        _dbContext.Groups.AddRange(groups);

        // Insert existing students into group to get "true" in [CreateGroupInfoDocxFile_GroupWithStudents]
        var newStudentInGroup = new Student
        {
            StudentFullName = "TestStudent22",
            CurrentGroupName = "GroupWithStudentsInIt",
            GroupId = groups.First(g => g.GroupName == "GroupWithStudentsInIt").GroupId
        };

        _dbContext.Students.Add(newStudentInGroup);
        await _dbContext.SaveChangesAsync();
    }
}