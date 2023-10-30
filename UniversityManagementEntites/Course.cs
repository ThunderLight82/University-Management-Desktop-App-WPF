using System.Collections.Generic;

namespace UniversityManagement.Entities;

public class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; }

    public List<Group> Groups { get; set; }

    public List<Teacher> Teachers { get; set; }

    public Course()
    {
        Groups = new List<Group>();
        Teachers = new List<Teacher>();
    }
}