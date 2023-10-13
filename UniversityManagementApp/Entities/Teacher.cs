namespace UniversityManagement.Entities;

public class Teacher
{
    public int TeacherId { get; set; }

    public string TeacherFullName { get; set; }

    public bool IsCorrespondence { get; set; }

    public string? CurrentGroupCurationName { get; set; }
}