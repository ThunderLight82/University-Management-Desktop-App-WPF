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

    // public string GroupCuratorDisplay
    // {
    //     get
    //     {
    //         if (GroupCurator == null || GroupCurator.Count == 0) return "";
    //
    //         return string.Join(", ", GroupCurator.Select(curator => curator.TeacherFullName));
    //     }
    // }

    public Group()
    {
        Students = new List<Student>();
        GroupCurator = new List<Teacher>();
    }
}