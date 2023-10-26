using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;

namespace UniversityManagement.Services;

public class StudentService
{
    private UniversityDbContext _dbContext;

    public StudentService(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool AddStudent(string studentFullName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(studentFullName))
                throw new ArgumentException();

            if (CheckIfStudentExists(studentFullName))
                throw new ArgumentNullException();
            
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
        catch
        {
            return false;
        }
    }

    public bool DeleteStudent(Student selectedStudent)
    {
        try
        {
            if (selectedStudent == null)
                throw new ArgumentNullException();

            var associatedGroup = _dbContext.Groups.FirstOrDefault(group =>
                group.Students.Contains(selectedStudent));

            if (associatedGroup != null)
            {
                associatedGroup.Students.Remove(selectedStudent);
            }

            selectedStudent.CurrentGroupName = null;

            _dbContext.Students.Remove(selectedStudent);

            _dbContext.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ChangeStudentNameAndWorkInfo(Student selectedStudent, string newFullName, bool isWorkingInDepartment)
    {
        try
        {
            if (selectedStudent == null)
                throw new ArgumentNullException();

            if (string.IsNullOrWhiteSpace(newFullName))
                throw new ArgumentException();

            selectedStudent.StudentFullName = newFullName;
            selectedStudent.IsWorkingInDepartment = isWorkingInDepartment;

            _dbContext.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool AddStudentsToGroup(Course selectedCourse, string selectedGroupName, List<Student> selectedStudents)
    {
        try
        {
            if (selectedCourse == null || string.IsNullOrWhiteSpace(selectedGroupName))
                throw new ArgumentNullException();

            if (selectedStudents.Count == 0)
                throw new InvalidOperationException();

            foreach (var student in selectedStudents)
            {
                if (string.IsNullOrWhiteSpace(student.StudentFullName))
                    throw new ArgumentException();

                var selectedGroup = _dbContext.Groups.Include(group =>
                    group.Students).FirstOrDefault(group => group.GroupName == selectedGroupName);

                if (string.IsNullOrWhiteSpace(student.CurrentGroupName))
                {
                    student.CurrentGroupName = selectedGroup.GroupName;
                    student.GroupId = selectedGroup.GroupId;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            
            _dbContext.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteStudentsFromGroup(List<Student> selectedStudents)
    {
        try
        {
            if (selectedStudents.Count == 0)
                throw new InvalidOperationException();

            foreach (var student in selectedStudents)
            {
                if (string.IsNullOrWhiteSpace(student.StudentFullName))
                    throw new ArgumentException();

                if (!string.IsNullOrWhiteSpace(student.CurrentGroupName))
                {
                    student.CurrentGroupName = null;
                    student.GroupId = null;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            _dbContext.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool CheckIfStudentExists(string studentFullName)
    {
        return _dbContext.Students.Any(student => student.StudentFullName == studentFullName);
    }

    public IEnumerable<Student> PopulateStudentList()
    {
        return new ObservableCollection<Student>(_dbContext.Students.ToList());
    }

    public IEnumerable<Course> PopulateCourseList()
    {
        return new ObservableCollection<Course>(_dbContext.Courses.ToList());
    }
}