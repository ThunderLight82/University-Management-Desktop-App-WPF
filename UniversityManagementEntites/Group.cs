using System.Collections.Generic;

namespace UniversityManagement.Entities;

public class Group
{
    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public int CourseId { get; set; }

    public Course Course { get; set; }

    public List<Student> Students { get; set; }

    public List<Teacher> GroupCurator { get; set; }

    public Group()
    {
        Students = new List<Student>();
        GroupCurator = new List<Teacher>();
    }
}