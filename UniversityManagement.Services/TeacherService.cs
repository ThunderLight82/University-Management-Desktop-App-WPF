using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;

namespace UniversityManagement.Services;

public class TeacherService
{
    private readonly UniversityDbContext _dbContext;

    public TeacherService (UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateTeacherAsync(string teacherFullName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(teacherFullName))
                throw new ArgumentException();

            if (CheckIfTeacherExists(teacherFullName))
                throw new DuplicateNameException();

            var newTeacher = new Teacher
            {
                TeacherFullName = teacherFullName,
                IsCorrespondence = false
            };

            _dbContext.Teachers.Add(newTeacher);

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteTeacherAsync(Teacher selectedTeacher)
    {
        try
        {
            if (selectedTeacher == null)
                throw new ArgumentNullException();

            var associatedGroup = _dbContext.Groups.FirstOrDefault(group =>
                group.GroupCurator.Contains(selectedTeacher));

            if (associatedGroup != null)
            {
                associatedGroup.GroupCurator.Remove(selectedTeacher);
            }

            selectedTeacher.CurrentGroupCurationName = null;

            _dbContext.Teachers.Remove(selectedTeacher);

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ChangeTeacherNameAndWorkInfoAsync(Teacher selectedTeacher, string newFullName, bool isCorrespondence)
    {
        try
        {
            if (selectedTeacher == null)
                throw new ArgumentNullException();

            if (string.IsNullOrWhiteSpace(newFullName)) 
                throw new ArgumentException();

            selectedTeacher.TeacherFullName = newFullName;
            selectedTeacher.IsCorrespondence = isCorrespondence;

            await _dbContext.SaveChangesAsync();

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

    public IEnumerable<Teacher> PopulateTeacherList()
    {
        return new ObservableCollection<Teacher>(_dbContext.Teachers.ToList());
    }
}