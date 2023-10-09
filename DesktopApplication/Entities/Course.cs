using System.Collections.Generic;

namespace DesktopApplication.Entities;

public class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; }

    public List<Group> Groups { get; set; }

    public List<Teacher> Teachers { get; set; }

    public int LastUsedGroupId { get; set; }

    // VV Need to delete that from table bcs it's have no real use VV
    public Course()
    {
        Groups = new List<Group>();
        Teachers = new List<Teacher>();
        LastUsedGroupId = 0;
    }
}