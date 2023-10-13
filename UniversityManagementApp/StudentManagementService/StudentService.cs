using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data;
using UniversityManagement.Entities;

namespace UniversityManagement.StudentManagementService;

public class StudentService
{
    private UniversityDbContext _dbContext;

    public StudentService(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool AddStudent(string studentFullName)
    {
        if (string.IsNullOrWhiteSpace(studentFullName))
        {
            MessageBox.Show("Please, enter a valid student name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        var newStudentNameAlreadyExists = _dbContext.Students.Any(student => 
            student.StudentFullName == studentFullName);

        if (newStudentNameAlreadyExists)
        {
            var duplicateStudentQuestion = MessageBox.Show(
                "A student with the same name already exists. Do you want to add this student anyway?",
                "Duplication name",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (duplicateStudentQuestion == MessageBoxResult.No)
            {
                return false;
            }
        }

        var newStudent = new Student
        {
            StudentFullName = studentFullName,
            IsWorkingInDepartment = false,
            CurrentGroupName = null,
            GroupId = null
        };

        _dbContext.Students.Add(newStudent);

        _dbContext.SaveChanges();

        return true;
    }

    public bool DeleteStudent(Student selectedStudent)
    {
        if (selectedStudent == null)
        {
            MessageBox.Show("Please, select a student from the list below to remove", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        var associatedGroup = _dbContext.Groups.FirstOrDefault(group => group.Students.Contains(selectedStudent));

        if (associatedGroup != null)
        {
            associatedGroup.Students.Remove(selectedStudent);
        }

        _dbContext.Students.Remove(selectedStudent);

        _dbContext.SaveChanges();

        return true;
    }

    public bool ChangeStudentNameAndWorkInfo(Student selectedStudent, string newFullName, bool isWorkingInDepartment)
    {
        if (selectedStudent == null)
        {
            MessageBox.Show("Please, select student from list first to update info", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        if (string.IsNullOrWhiteSpace(newFullName))
        {
            MessageBox.Show("Please, enter a valid student name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        selectedStudent.StudentFullName = newFullName;
        selectedStudent.IsWorkingInDepartment = isWorkingInDepartment;

        _dbContext.SaveChanges();

        return true;
    }

    public bool AddStudentsToGroup(Course selectedCourse, string selectedGroupName, List<Student> selectedStudents)
    {
        if (selectedCourse == null || string.IsNullOrWhiteSpace(selectedGroupName))
        {
            MessageBox.Show("Please, select both a course and a group to add students to", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        var selectedGroup = _dbContext.Groups.Include(group =>
            group.Students).FirstOrDefault(group => group.GroupName == selectedGroupName);

        if (selectedGroup == null)
        {
            MessageBox.Show("The selected group does not exist for the chosen course", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        if (selectedStudents.Count > 0)
        {
            foreach (var student in selectedStudents)
            {
                if (string.IsNullOrWhiteSpace(student.CurrentGroupName))
                {
                    student.CurrentGroupName = selectedGroup.GroupName;
                    student.GroupId = selectedGroup.GroupId;
                }
                else
                {
                    MessageBox.Show("Student '" + student.StudentFullName + "' is already assigned to a group '" + student.CurrentGroupName + "'", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }
        }
        else
        {
            MessageBox.Show("Please, select a student from the list to add to the group", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        _dbContext.SaveChanges();

        return true;
    }
    
    public bool DeleteStudentsFromGroup(List<Student> selectedStudents)
    {
        if (selectedStudents.Count > 0)
        {
            foreach (var student in selectedStudents)
            {
                if (!string.IsNullOrWhiteSpace(student.CurrentGroupName))
                {
                    student.CurrentGroupName = null;
                    student.GroupId = null;
                }
                else
                {
                    MessageBox.Show("This student is not assigned to any group", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }
        }
        else
        {
            MessageBox.Show("Please, select a student from the list to remove from the group", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        _dbContext.SaveChanges();

        return true;
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

    public async Task<bool> ImportStudents(Course selectedCourse, string selectedGroupName, string filePath)
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