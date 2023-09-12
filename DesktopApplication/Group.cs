using System.Collections.Generic;

namespace DesktopApplication;

public class Group
{
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public List<Student> Students { get; set; }
    public string GroupCuratorName { get; set; }
    public Teacher GroupCurator { get; set; }
    
    public Group()
    {
        Students = new List<Student>();
    }
}