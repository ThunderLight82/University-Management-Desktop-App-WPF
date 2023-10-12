namespace DesktopApplication.Entities;

public class Student
{
    public int StudentId { get; set; }

    public string StudentFullName { get; set; }

    public bool IsWorkingInDepartment { get; set; }

    public string? CurrentGroupName { get; set; }

    public int? GroupId { get; set; }
}