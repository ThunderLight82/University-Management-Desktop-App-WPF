using Microsoft.EntityFrameworkCore;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;
using Xunit;

namespace UniversityManagement.UnitTests;

public class TeacherServiceTests
{
    private readonly UniversityDbContext _dbContext;
    private readonly TeacherService _teacherService;

    public TeacherServiceTests()
    {
        var options = new DbContextOptionsBuilder<UniversityDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDBUniversityDatabase")
            .Options;

        _dbContext = new UniversityDbContext(options);
        _teacherService = new TeacherService(_dbContext);
    }

    [Theory]
    [InlineData("NewTeacher", true)]
    [InlineData("1!NewTeacher1.'1", true)]
    [InlineData("якезещиієєєєц3532а23н", true)]
    [InlineData("фзфвз2* 5hn5 4h   %__<><><fjи", true)]
    [InlineData(" .  ", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData(null, false)]
    [InlineData("TeacherNameDuplicationTest", false)]
    public async Task CreateTeacherAsync_DifferentNamesInputs_ShowExpectedResult(
        string newTeacherName, 
        bool expectedTeacherCreationResult)
    {
        //Arrange
        await FillTeachersTestsWithAbstractDataAsync();

        //Act
        var creationResult = await _teacherService.CreateTeacherAsync(newTeacherName);

        //Assert
        Assert.Equal(expectedTeacherCreationResult, creationResult);
    }

    [Theory]
    [InlineData("TeacherToDelete", true)]
    [InlineData("TeacherWithGroup", true)]
    [InlineData("[)?{>?_?=-Second__+_+GroupTeacher gb8rh 9w 2", true)]
    [InlineData(" ", true)]
    [InlineData("", true)]
    [InlineData(null, false)]
    public async Task DeleteTeacherAsync_DifferentDeletionsVariants_ShowExpectedResult(
        string teacherToDeleteFullName, 
        bool expectedTeacherDeletionResult)
    {
        //Arrange
        await FillTeachersTestsWithAbstractDataAsync();

        var teacherToDelete = _dbContext.Teachers.FirstOrDefault(t => t.TeacherFullName == teacherToDeleteFullName);

        //Act
        var deletionResult = await _teacherService.DeleteTeacherAsync(teacherToDelete);

        //Assert
        Assert.Equal(expectedTeacherDeletionResult, deletionResult);

        //case for clearing "GroupCurator" and "CurrentGroupCurationName" field when deleted in Group and Teacher entity.
        if (teacherToDeleteFullName == "TeacherWithGroup" && expectedTeacherDeletionResult)
        {
            var associatedGroup = _dbContext.Groups.FirstOrDefault(group => group.GroupCurator.Contains(teacherToDelete));
            var teacherStillHasCuration = teacherToDelete.CurrentGroupCurationName != null;

            Assert.Null(associatedGroup);
            Assert.False(teacherStillHasCuration);
        }
    }

    [Theory]
    [InlineData("TeacherToChange", "ChangedTeacherName", false, false, true)]
    [InlineData("TeacherToChangeWithCorrespondence", " 34 32 ChangedTeacherNameAndCorrespondence  113", true, false, true)]
    [InlineData("fw     34gTeacherToChange       ><>?", " ", false, true, false)]
    [InlineData("", "", false, false, false)]
    [InlineData(" ", "NewTeacherNameAdnNullCheck", null, true, true)]
    [InlineData(null, "OnlyNullCheck", null, null, false)]
    [InlineData(null, null, true, false, false)]
    [InlineData("ChangeToNull", null,false, false, false)]
    public async Task ChangeTeacherNameAndWorkInfoAsync_DifferentChangesInNameAndWorkInfo_ShowExpectedResult(
        string currentTeacherFullName,
        string newTeacherFullName,
        bool? currentCorrespondence, 
        bool newCorrespondence,
        bool expectedTeacherChangesResult)
    {
        //Arrange
        await FillTeachersTestsWithAbstractDataAsync();

        var teacherToChange = _dbContext.Teachers.FirstOrDefault(t => t.TeacherFullName == currentTeacherFullName);
        
        var originalTeacherFullName = teacherToChange?.TeacherFullName;

        //Act
        var changeResult = await _teacherService.ChangeTeacherNameAndWorkInfoAsync(teacherToChange, newTeacherFullName, newCorrespondence);

        //Assert
        Assert.Equal(expectedTeacherChangesResult, changeResult);
        
        if (changeResult)
        {
            Assert.Equal(newTeacherFullName, teacherToChange.TeacherFullName);
        }
        else
        {
            Assert.Equal(currentTeacherFullName, originalTeacherFullName);
        }
    }

    private async Task FillTeachersTestsWithAbstractDataAsync()
    {
        var teachers = new List<Teacher>
        {
            // case for [CreateTeacher] tests.
            new() { TeacherFullName = "TeacherNameDuplicationTest" },
            // cases for [DeleteTeacher] tests.
            new() { TeacherFullName = "TeacherToDelete" },
            new() { TeacherFullName = "TeacherWithGroup", CurrentGroupCurationName = "ExistingGroupCuration" },
            new() { TeacherFullName = "[)?{>?_?=-Second__+_+GroupTeacher gb8rh 9w 2" },
            new() { TeacherFullName = " "},
            new() { TeacherFullName = "" },
            // cases for [ChangeTeacherNameAndWorkInfo] tests.
            new() { TeacherFullName = "TeacherToChange", IsCorrespondence = false },
            new() { TeacherFullName = "TeacherToChangeWithCorrespondence", IsCorrespondence = true },
            new() { TeacherFullName = "fw     34gTeacherToChange       ><>?", IsCorrespondence = false},
            new() { TeacherFullName = "ChangeToNull", IsCorrespondence = false}
        };

        _dbContext.Teachers.AddRange(teachers);
        await _dbContext.SaveChangesAsync();
    }
}