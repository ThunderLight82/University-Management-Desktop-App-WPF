using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DesktopApplication;
    
public class Course
{ 
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public ObservableCollection<Group> Groups { get; set; }
    public List<Teacher> Teachers { get; set; }
    public int LastUsedGroupId { get; set; }

    public Course()
    {
        Groups = new ObservableCollection<Group>();
        Teachers = new List<Teacher>();
        LastUsedGroupId = 0;
    }
}




