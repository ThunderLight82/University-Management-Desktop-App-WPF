﻿using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.UnitTests;

public class GroupServiceTests
{
    private readonly UniversityDbContext _dbContext;
    private readonly GroupService _groupService;
    private readonly Course _selectedCourse;

    public GroupServiceTests()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var options = new DbContextOptionsBuilder<UniversityDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDBUniversityDatabase")
            .Options;

        _dbContext = new UniversityDbContext(options);
        _groupService = new GroupService(_dbContext);

        _selectedCourse = new Course
        {
            CourseName = "TestCourse3"
        };

        _dbContext.Courses.Add(_selectedCourse);
        _dbContext.SaveChanges();
    }

    [Theory]
    [InlineData("New TestGroup1", true)]
    [InlineData("1!NewTestGroup1", true)]
    [InlineData("якезещиієєєєц3532а23н", true)]
    [InlineData("фзфвз2*    ^%__+-_+&*#$:{  gfr><><><< 4SSsініеіуіди", true)]
    [InlineData(" .  ", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData(null, false)]
    [InlineData("GroupNameDuplicationTest", false)]
    public async Task CreateGroupAsync_DifferentNamesInputs_ShowExpectedResult(
        string groupName,
        bool expectedGroupCreationResult)
    {
        //Arrange
        await FillGroupsTestsWithAbstractDataAsync(_selectedCourse);

        //Act
        var creationResult = await _groupService.CreateGroupAsync(_selectedCourse, groupName);

        //Assert
        Assert.Equal(expectedGroupCreationResult, creationResult);
    }

    [Theory]
    [InlineData("DeleteTestGroup1", true)]
    [InlineData("    1-20-2-Delete New Test Group 2", true)]
    [InlineData("GroupWithStudentsInIt", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public async Task DeleteGroupAsync_DifferentDeletionsVariants_ShowExpectedResult(
        string groupToDeleteName,
        bool expectedGroupDeletionResult)
    {
        //Arrange
        await FillGroupsTestsWithAbstractDataAsync(_selectedCourse);

        //Act
        var deletionResult = await _groupService.DeleteGroupAsync(_selectedCourse, groupToDeleteName);

        //Assert
        Assert.Equal(expectedGroupDeletionResult, deletionResult);
    }

    [Theory]
    [InlineData("GroupToRename", "!RenamedGroup10101.", true)]
    [InlineData("TrimCheck", "   !RenamedGroup10101.         ", true)]
    [InlineData("-;;-[;g-=4;g4=0 30f 3-0f/'/. f3a", "gf 4pg leg0,  d0mf we 31>?>>:#%^$%%", true)]
    [InlineData(" ", ",", false)]
    [InlineData("", "", false)]
    [InlineData("NullTest", null, false)]
    [InlineData(null, null, false)]
    public async Task EditGroupNameAsync_DifferentNewNamesVariants_ShowExpectedResult(
        string currentGroupName,
        string newGroupName,
        bool expectedGroupRenameResult)
    {
        //Arrange
        await FillGroupsTestsWithAbstractDataAsync(_selectedCourse);

        var originalGroupName = _selectedCourse.Groups.FirstOrDefault(group =>
            group.GroupName == currentGroupName)?.GroupName;

        //Act
        var renamingResult = await _groupService.EditGroupNameAsync(_selectedCourse, currentGroupName, newGroupName);

        var renamedGroupName = _selectedCourse.Groups.FirstOrDefault(group =>
            group.GroupName == newGroupName)?.GroupName;

        //Assert
        Assert.Equal(expectedGroupRenameResult, renamingResult);

        if (renamingResult)
        {
            Assert.Equal(newGroupName, renamedGroupName);
        }
        else
        {
            Assert.Equal(currentGroupName,originalGroupName);
        }
    }

    [Theory]
    [InlineData("GroupToAssignNewCuration", "FirstGroupTeacher", true)]
    [InlineData("GroupToReassignCuration", "TeacherWithGroup", true)]
    [InlineData("           WEIRDNAMEGROZPYUhb 4 g4o defm 21 ", "[)?{>?_?=-Second__+_+GroupTeacher gb8rh 9w 2", true)]
    [InlineData(" ", " ", false)]
    [InlineData("", "SecondGroupTeacher", false)]
    [InlineData(null, "TeacherExistButNotGroup", false)]
    [InlineData("GroupExistButNotTeacher", null, false)]
    [InlineData(null, null, false)]
    public async Task SelectGroupCuratorAsync_DifferentCurationVariants_ShowExpectedResult(
        string groupName,
        string teacherFullName,
        bool expectedGroupCurationAssignResult)
    {
        //Arrange
        await FillGroupsTestsWithAbstractDataAsync(_selectedCourse);

        var teacherToAssign = _dbContext.Teachers.FirstOrDefault(t => t.TeacherFullName == teacherFullName);

        //Act
        var curationAssignResult = await _groupService.SelectGroupCuratorAsync(_selectedCourse, teacherToAssign, groupName);

        //Assert
        Assert.Equal(expectedGroupCurationAssignResult, curationAssignResult);
    }

    private async Task FillGroupsTestsWithAbstractDataAsync(Course selectedCourse)
    {
        var groups = new List<Group>
        {
            // case for [CreateGroup] test.
            new() { GroupName = "GroupNameDuplicationTest", CourseId = selectedCourse.CourseId },
            // cases for [DeleteGroup] tests.
            new() { GroupName = "DeleteTestGroup1", CourseId = selectedCourse.CourseId },
            new() { GroupName = "    1-20-2-Delete New Test Group 2", CourseId = selectedCourse.CourseId },
            new() { GroupName = "GroupWithStudentsInIt", CourseId = selectedCourse.CourseId },
            new() { GroupName = "", CourseId = selectedCourse.CourseId },
            // cases for [EditGroupName] tests.
            new() { GroupName = "GroupToRename", CourseId = selectedCourse.CourseId },
            new() { GroupName = "TrimCheck", CourseId = selectedCourse.CourseId },
            new() { GroupName = "-;;-[;g-=4;g4=0 30f 3-0f/'/. f3a", CourseId = selectedCourse.CourseId },
            new() { GroupName = " ", CourseId = selectedCourse.CourseId },
            new() { GroupName = "NullTest", CourseId = selectedCourse.CourseId },
            // cases for [SelectGroupCurator] tests.
            new() { GroupName = "GroupToReassignCuration", CourseId = selectedCourse.CourseId },
            new() { GroupName = "GroupToAssignNewCuration", CourseId = selectedCourse.CourseId },
            new() { GroupName = "           WEIRDNAMEGROZPYUhb 4 g4o defm 21 ", CourseId = selectedCourse.CourseId },
            new() { GroupName = "GroupExistButNotTeacher", CourseId = selectedCourse.CourseId },
        };
        
        _dbContext.Groups.AddRange(groups);

        // Insert existing students into group to get "false" in [DeleteGroup] case.
        var newStudentInGroup = new Student
        {
            StudentFullName = "TestStudent",
            CurrentGroupName = "GroupWithStudentsInIt",
            GroupId = groups.First(g => g.GroupName == "GroupWithStudentsInIt").GroupId
        };

        _dbContext.Students.Add(newStudentInGroup);

        // Insert existing teacher into context to get correct results in [SelectGroupCurator] tests.
        var teachersToAssign = new List<Teacher>
        {
            new() { TeacherFullName = "FirstGroupTeacher" },
            new() { TeacherFullName = "TeacherWithGroup", CurrentGroupCurationName = "ExistingGroupCuration" },
            new() { TeacherFullName = "[)?{>?_?=-Second__+_+GroupTeacher gb8rh 9w 2" },
            new() { TeacherFullName = " " },
            new() { TeacherFullName = "SecondGroupTeacher" },
            new() { TeacherFullName = "TeacherExistButNotGroup" },
        };

        _dbContext.Teachers.AddRange(teachersToAssign);
        await _dbContext.SaveChangesAsync();
    }
}