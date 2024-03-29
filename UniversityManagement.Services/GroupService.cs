﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;

namespace UniversityManagement.Services;

public class GroupService
{
    private readonly UniversityDbContext _dbContext;

    public GroupService(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateGroupAsync(Course selectedCourse, string groupName)
    {
        try
        {
            if (selectedCourse == null || string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentNullException();

            var groupNameExist = selectedCourse.Groups.Any(group =>
                group.GroupName == groupName);

            if (groupNameExist)
                throw new DuplicateNameException();

            var createNewGroup = new Group { GroupName = groupName };

            selectedCourse.Groups.Add(createNewGroup);

            _dbContext.Groups.Add(createNewGroup);

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteGroupAsync(Course selectedCourse, string groupName)
    {
        try
        {
            if (selectedCourse == null || string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentNullException();

            var deleteGroup = selectedCourse.Groups.FirstOrDefault(group =>
                group.GroupName == groupName);

            if (deleteGroup!.Students.Count == 0)
            {
                foreach (var teacher in deleteGroup.GroupCurator)
                {
                    teacher.CurrentGroupCurationName = null;
                }

                selectedCourse.Groups.Remove(deleteGroup);

                _dbContext.Groups.Remove(deleteGroup);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException();
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> EditGroupNameAsync(Course selectedCourse, string currentGroupName, string newGroupName)
    {
        try
        {
            if (selectedCourse == null || string.IsNullOrWhiteSpace(currentGroupName))
                throw new ArgumentNullException();

            if (string.IsNullOrWhiteSpace(newGroupName))
                throw new ArgumentException();

            var editGroupName = selectedCourse.Groups.FirstOrDefault(group =>
                group.GroupName == currentGroupName);

            editGroupName!.GroupName = newGroupName;

            var studentsToUpdate = _dbContext.Students.Where(s =>
                s.CurrentGroupName == currentGroupName);

            foreach (var student in studentsToUpdate)
            {
                student.CurrentGroupName = newGroupName;
            }

            var teachersToUpdate = _dbContext.Teachers.Where(t =>
                t.CurrentGroupCurationName == currentGroupName);

            foreach (var teacher in teachersToUpdate)
            {
                teacher.CurrentGroupCurationName = newGroupName;
            }

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SelectGroupCuratorAsync(Course selectedCourse, Teacher teacherToAssign, string groupName)
    {
        try
        {
            if (teacherToAssign == null || string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentNullException();

            var selectedGroup = selectedCourse.Groups.FirstOrDefault(group =>
                group.GroupName == groupName);

            selectedGroup!.GroupCurator.Add(teacherToAssign);

            teacherToAssign.CurrentGroupCurationName = groupName;

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<Course> PopulateCourseList()
    {
        return new ObservableCollection<Course>(_dbContext.Courses.ToList());
    }

    public IEnumerable<Teacher> PopulateTeacherList()
    {
        return new ObservableCollection<Teacher>(_dbContext.Teachers.ToList());
    }
}