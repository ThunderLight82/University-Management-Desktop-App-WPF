using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;

namespace UniversityManagement.Services;

public class TeacherService
{
    private UniversityDbContext _dbContext;

    public TeacherService (UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Teacher> PopulateTeacherList()
    {
        return new ObservableCollection<Teacher>(_dbContext.Teachers.ToList());
    }

    public bool CreateTeacher(string teacherFullName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(teacherFullName))
                throw new ArgumentException();

            if (_dbContext.Teachers.Any(teacher => teacher.TeacherFullName == teacherFullName))
                throw new DuplicateNameException();

            var newTeacher = new Teacher
            {
                TeacherFullName = teacherFullName,
                IsCorrespondence = false
            };

            _dbContext.Teachers.Add(newTeacher);

            _dbContext.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool CheckIfTeacherExists(string teacherFullName)
    {
        return _dbContext.Teachers.Any(teacher => teacher.TeacherFullName == teacherFullName);
    }

    public bool DeleteTeacher(Teacher selectedTeacher)
    {
        try
        {
            if (selectedTeacher == null)
                throw new ArgumentNullException();

            var associatedGroup = _dbContext.Groups.FirstOrDefault(group => group.GroupCurator.Contains(selectedTeacher));

            if (associatedGroup != null)
            {
                associatedGroup.GroupCurator.Remove(selectedTeacher);
            }

            selectedTeacher.CurrentGroupCurationName = null;

            _dbContext.Teachers.Remove(selectedTeacher);

            _dbContext.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ChangeTeacherNameAndWorkInfo(Teacher selectedTeacher, string newFullName, bool isCorrespondence)
    {
        try
        {
            if (selectedTeacher == null)
                throw new ArgumentNullException();

            if (string.IsNullOrWhiteSpace(newFullName)) 
                throw new ArgumentException();

            selectedTeacher.TeacherFullName = newFullName;
            selectedTeacher.IsCorrespondence = isCorrespondence;

            _dbContext.SaveChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }
}