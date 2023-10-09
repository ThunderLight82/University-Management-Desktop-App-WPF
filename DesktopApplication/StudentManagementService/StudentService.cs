using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DesktopApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesktopApplication;

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
            CurrentGroupName = "",
            GroupId = 0
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

        selectedStudent.CurrentGroupName = "";
        selectedStudent.GroupId = 0;

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
                    selectedGroup.Students.Add(student);
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
                    student.CurrentGroupName = "";
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

    public bool ImportStudents(Student student)
    {
        return true;
    }

    public bool ExportStudents(Student student)
    {
        return true;
    }
}