using Microsoft.EntityFrameworkCore;
using System.Text;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;
using UniversityManagement.Services;
using Xunit;

namespace UniversityManagement.UnitTests;

public class StudentServiceTests
{
    private readonly UniversityDbContext _dbContext;
    private readonly StudentService _studentService;
    private readonly Course _selectedCourse;

    public StudentServiceTests()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var options = new DbContextOptionsBuilder<UniversityDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDBUniversityDatabase")
            .Options;

        _dbContext = new UniversityDbContext(options);
        _studentService = new StudentService(_dbContext);

        _selectedCourse = new Course
        {
            CourseName = "TestCourse1",
        };

        _dbContext.Courses.Add(_selectedCourse);
        _dbContext.SaveChanges();
    }

    [Theory]
    [InlineData("NewTestStudent", true)]
    [InlineData("           1!      NewTestStudent3 f3- b590 me4059bjk 3 s1", true)]
    [InlineData("якезещ32а23н", true)]
    [InlineData(" .  ", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData(null, false)]
    [InlineData("StudentNameDuplicationTest", false)]
    public void AddStudent_DifferentNamesInputs_ShowExpectedResult(
        string newStudentName, 
        bool expectedStudentCreationResult)
    {
        //Arrange
        FillStudentsTestsWithAbstractData(_selectedCourse);

        //Act
        var creationResult = _studentService.AddStudent(newStudentName);

        //Assert
        Assert.Equal(expectedStudentCreationResult, creationResult);
    }

    [Theory]
    [InlineData("StudentToDelete", true)]
    [InlineData("StudentWithinGroupTest2", true)]
    [InlineData("   [  )?{>?_?=-Secondvebe_0Studen4bv4bf gb8rh 9w 2  ", true)]
    [InlineData(",", true)]
    [InlineData(" ", true)]
    [InlineData("", true)]
    [InlineData(null, false)]
    public void DeleteStudent_DifferentDeletionsVariants_ShowExpectedResult(
        string studentToDeleteFullName,
        bool expectedStudentDeletionResult)
    {
        //Arrange
        FillStudentsTestsWithAbstractData(_selectedCourse);

        var studentToDelete = _dbContext.Students.FirstOrDefault(s => s.StudentFullName == studentToDeleteFullName);

        //Act
        var deletionResult = _studentService.DeleteStudent(studentToDelete);

        //Assert
        Assert.Equal(expectedStudentDeletionResult, deletionResult);

        //case for clearing "Student" and "CurrentGroupName" in field when deleted in Group and Student entity.
        if (studentToDeleteFullName == "StudentWithinGroupTest2" && expectedStudentDeletionResult)
        {
            var associatedGroup = _dbContext.Groups.FirstOrDefault(group => group.Students.Contains(studentToDelete));
            var studentStillWithinGroup = studentToDelete.CurrentGroupName != null;

            Assert.Null(associatedGroup);
            Assert.False(studentStillWithinGroup);
        }
    }

    [Theory]
    [InlineData("StudentToChange", "ChangedStudentName", false, false, true)]
    [InlineData("StudentToChangeWithDepartmentWorking", " 34 32 ChangedStudentNameAndWorkInfo  113", true, false, true)]
    [InlineData("fw     34gStudentToChange       ><>?", " ", false, true, false)]
    [InlineData("", "", false, false, false)]
    [InlineData(" ", "NewStudentNameAdnNullCheck", null, true, true)]
    [InlineData(null, "OnlyNullCheck", null, null, false)]
    [InlineData(null, null, true, false, false)]
    [InlineData("ChangeToNull", null, false, false, false)]
    public void ChangeStudentNameAndWorkInfo_DifferentChangesInNameAndWorkInfo_ShowExpectedResult(
        string currentStudentFullName,
        string newStudentFullName,
        bool? currentWorkingInDepartment,
        bool newWorkingInDepartment,
        bool expectedStudentChangesResult)
    {
        //Arrange
        FillStudentsTestsWithAbstractData(_selectedCourse);

        var studentToChange = _dbContext.Students.FirstOrDefault(s => s.StudentFullName == currentStudentFullName);

        //Act
        var changeResult = _studentService.ChangeStudentNameAndWorkInfo(studentToChange, newStudentFullName, newWorkingInDepartment);

        //Assert
        Assert.Equal(expectedStudentChangesResult, changeResult);
    }
    
    [Theory]
    [InlineData("EmptyGroup", "StudentToAdd1", true)]
    [InlineData("GroupWithSomeStudent", "StudentToAdd2", true)]
    [InlineData("GroupWithSomeStudent", "StudentWithNullInGroup", true)]
    [InlineData(",   Gro  upW  ithSo   meS    tudent   .", "ervwevberb Syrgb kbnm 4;", true)]
    //VV Need to fix that VV
    [InlineData("GroupWithSomeStudent", "StudentWithinGroupTest", false)]
    [InlineData("drhe e45g w yW$YU4e3g $~!!~#", "", false)]
    [InlineData(" ", "WhiteSpaceGroup", false)]
    [InlineData(" ", " ",  false)]
    [InlineData("", "",  false)]
    [InlineData(null, "StudentWithNullInGroup", false)]
    [InlineData(null, null,  false)]
    public void AddStudentsToGroup_DifferentStudentsAddingVariants_ShowExpectedResult(
        string selectedGroupName,
        string studentFullName,
        bool expectedStudentAddToGroupResult)
    {
        //Arrange
        FillStudentsTestsWithAbstractData(_selectedCourse);

        var studentToAddInGroup = _dbContext.Students.FirstOrDefault(s => s.StudentFullName == studentFullName);

        var studentsToAdd = new List<Student> { studentToAddInGroup };

        //Act
        var addedResult = _studentService.AddStudentsToGroup(_selectedCourse, selectedGroupName, studentsToAdd);

        //Assert
        Assert.Equal(expectedStudentAddToGroupResult, addedResult);
    }

    [Theory]
    [InlineData("StudentWithinSOMEGroupTest", true)]
    [InlineData("DeleteFromGroupTestWithNullInGroup", false)]
    [InlineData("DeleteFromGroupTestWithNothingInGroupName", false)]
    public void DeleteStudentsFromGroup_DifferentStudentsDeleteVariants_ShowExpectedResult(
        string selectedStudentToDelete,
        bool expectedStudentDeleteFromGroupResult)
    {
        //Arrange
        FillStudentsTestsWithAbstractData(_selectedCourse);

        var studentToRemoveFromGroup = _dbContext.Students.FirstOrDefault(s => s.StudentFullName == selectedStudentToDelete);

        var studentsToRemove = new List<Student> { studentToRemoveFromGroup };

        //Act
        var deletedResult = _studentService.DeleteStudentsFromGroup(studentsToRemove);

        //Assert
        Assert.Equal(expectedStudentDeleteFromGroupResult, deletedResult);
    }

    [Theory]
    [InlineData("StudentNotInSelectedGroup", false)]
    public void DeleteStudentsFromGroup_StudentNotSelected_ShowExpectedResult(
        string selectedStudentToDelete,
        bool expectedStudentDeleteFromGroupResult)
    {
        // Arrange
        FillStudentsTestsWithAbstractData(_selectedCourse);

        // Act
        var deletedResult = _studentService.DeleteStudentsFromGroup(new List<Student> { new() { StudentFullName = selectedStudentToDelete } });

        // Assert
        Assert.Equal(expectedStudentDeleteFromGroupResult, deletedResult);
    }


    private void FillStudentsTestsWithAbstractData(Course selectedCourse)
    {
        var students = new List<Student>
        {
            // case for [AddStudent] tests.
            new() { StudentFullName = "StudentNameDuplicationTest" },
            // cases for [DeleteStudent] tests.
            new() { StudentFullName = "StudentToDelete" },
            new() { StudentFullName = "StudentWithinGroupTest2", CurrentGroupName = "GroupWithSomeStudent"},
            new() { StudentFullName = "   [  )?{>?_?=-Secondvebe_0Studen4bv4bf gb8rh 9w 2  " },
            new() { StudentFullName = "," },
            new() { StudentFullName = " " },
            new() { StudentFullName = "" },
            // cases for [ChangeStudentNameAndWorkInfo] tests.
            new() { StudentFullName = "StudentToChange", IsWorkingInDepartment = true },
            new() { StudentFullName = "StudentToChangeWithDepartmentWorking", IsWorkingInDepartment = false },
            new() { StudentFullName = "fw     34gStudentToChange       ><>?", IsWorkingInDepartment = true },
            new() { StudentFullName = "ChangeToNull", IsWorkingInDepartment = true },
            // cases for [AddStudentsToGroup] tests.
            new() { StudentFullName = "StudentToAdd1"},
            new() { StudentFullName = "StudentToAdd2" },
            new() { StudentFullName = "StudentWithinGroupTest", CurrentGroupName = "GroupWithSomeStudent" },
            new() { StudentFullName = "StudentWithNullInGroup", CurrentGroupName = null},
            new() { StudentFullName = "ervwevberb Syrgb kbnm 4;"},
            new() { StudentFullName = "WhiteSpaceGroup" },
            // cases for [DeleteStudentsFromGroup] tests.
            new() { StudentFullName = "StudentWithinSOMEGroupTest", CurrentGroupName = "GroupWithSomeStudent" },
            new() { StudentFullName = "DeleteFromGroupTestWithNullInGroup", CurrentGroupName = null},
            new() { StudentFullName = "DeleteFromGroupTestWithNothingInGroupName" }
        };

        _dbContext.Students.AddRange(students);
        _dbContext.SaveChanges();

        var groups = new List<Group>
        {
            // cases for [AddStudentsToGroup] test.
            new() { GroupName = "EmptyGroup", CourseId = selectedCourse.CourseId },
            new() { GroupName = "GroupWithSomeStudent", CourseId = selectedCourse.CourseId },
            new() { GroupName = ",   Gro  upW  ithSo   meS    tudent   .", CourseId = selectedCourse.CourseId },
            new() { GroupName = "drhe e45g w yW$YU4e3g $~!!~#", CourseId = selectedCourse.CourseId },
            new() { GroupName = " ", CourseId = selectedCourse.CourseId },
            new() { GroupName = "", CourseId = selectedCourse.CourseId }
        };

        _dbContext.Groups.AddRange(groups);
        _dbContext.SaveChanges();
    }
}