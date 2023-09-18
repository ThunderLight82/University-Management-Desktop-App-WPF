using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DesktopApplication;

// Data repository represent a abstract DB or existing info about courses, groups, etc. when working with data in app.
// It may heavily change or modify when we need to to use a real DB in practice. For now - it's just for showcase and testing.

public class DataRepository
{
    public List<Course> Courses { get; set; }
    public List<Teacher> Teachers { get; set; }
    public ObservableCollection<Group> Groups { get; set; }
    public List<Student> Students { get; set; }

    public DataRepository()
    {
        InitializeData();
    }

    private void InitializeData()
    {
        // VVV Hardcoded courses list below only for testings(showcase) purpose. VVV
        Courses = new List<Course>
        {
            new() { CourseId = 1, CourseName = "System Engineer" },
            new() { CourseId = 2, CourseName = "Software Engineer" },
            new() { CourseId = 3, CourseName = "Data Science" },
            new() { CourseId = 4, CourseName = "Data Analysis"},
            new() { CourseId = 5, CourseName = "Cyber Security"}
        };
        
        // VVV Hardcoded teachers list below only for testings(showcase) purpose. VVV
        Teachers = new List<Teacher>
        {
            new() { TeacherId = 1, TeacherFullName = "Sergiy Ponomarenko", IsCorrespondence = true },
            new() { TeacherId = 2, TeacherFullName = "Dana Mayhen", IsCorrespondence = false },
            new() { TeacherId = 3, TeacherFullName = "Oleksii Kovalov", IsCorrespondence = true },
            new() { TeacherId = 4, TeacherFullName = "Daniel Ostrowski", IsCorrespondence = true },
            new() { TeacherId = 5, TeacherFullName = "Ustam Yapir", IsCorrespondence = false },
            new() { TeacherId = 6, TeacherFullName = "Morar Krasnoportrko", IsCorrespondence = false }
        };

        // VVV Hardcoded students list below only for testings(showcase) purpose. VVV
        Students = new List<Student>
        {
            new() {StudentId = 1, StudentFullName = "Oleg Nekrasov", IsWorkingInDepartment = false},
            new() {StudentId = 2, StudentFullName = "Sergiy Cnkira", IsWorkingInDepartment = false},
            new() {StudentId = 3, StudentFullName = "Rosa Maksymenko", IsWorkingInDepartment = false},
            new() {StudentId = 4, StudentFullName = "Ella Bliss", IsWorkingInDepartment = false},
            new() {StudentId = 5, StudentFullName = "Hryhoriy Mulan", IsWorkingInDepartment = true},
            new() {StudentId = 6, StudentFullName = "Olexander Doberman", IsWorkingInDepartment = false},
            new() {StudentId = 7, StudentFullName = "Olexandra Minushevich", IsWorkingInDepartment = false},
            new() {StudentId = 8, StudentFullName = "Anna Samoilenko", IsWorkingInDepartment = false},
            new() {StudentId = 9, StudentFullName = "Ornieda Saakashvili", IsWorkingInDepartment = false},
            new() {StudentId = 10, StudentFullName = "Maxym Rubin", IsWorkingInDepartment = false},
            new() {StudentId = 11, StudentFullName = "Igor Kostin", IsWorkingInDepartment = false}
        };
        
        // VVV Hardcoded groups list below only for testings(showcase) purpose. VVV
        Groups = new ObservableCollection<Group>()
        {
            new() { GroupId = 111, GroupName = "SSE-11" },
            new() { GroupId = 112, GroupName = "SSE-31" },
            new() { GroupId = 113, GroupName = "SSE-22" },
            new() { GroupId = 221, GroupName = "SWE-41" },
            new() { GroupId = 222, GroupName = "SWE-32" },
            new() { GroupId = 331, GroupName = "DS-12" },
            new() { GroupId = 332, GroupName = "DS-21" },
            new() { GroupId = 441, GroupName = "DA-31" },
            new() { GroupId = 551, GroupName = "CS-31" },
            new() { GroupId = 552, GroupName = "CS-32" },
            new() { GroupId = 553, GroupName = "CS-41" }
        };
        
        Courses[0].Groups.Add(Groups[0]);
        Courses[0].Groups.Add(Groups[1]);
        Courses[0].Groups.Add(Groups[2]);
        
        Courses[1].Groups.Add(Groups[3]);
        Courses[1].Groups.Add(Groups[4]);
        
        Courses[2].Groups.Add(Groups[5]);
        Courses[2].Groups.Add(Groups[6]);
        
        Courses[3].Groups.Add(Groups[7]);
        
        Courses[4].Groups.Add(Groups[8]);
        Courses[4].Groups.Add(Groups[9]);
        Courses[4].Groups.Add(Groups[10]);
    }
}