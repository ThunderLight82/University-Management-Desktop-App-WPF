using System.Collections.Generic;

namespace DesktopApplication;

public class DataRepository
{
    public List<Course> Courses { get; set; }

    public DataRepository()
    {
        Courses = new List<Course>();
    }
}