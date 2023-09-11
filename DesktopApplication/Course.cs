using System.Collections.Generic;

namespace DesktopApplication;
    
public class Course
{ 
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public List<Group> Groups { get; set; }
    public int LastUsedGroupId { get; set; }

    public Course()
    {
        Groups = new List<Group>();
        LastUsedGroupId = 0;
    }
}




