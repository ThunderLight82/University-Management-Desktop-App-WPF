using System.Linq;
using System.Windows;
using UniversityManagement.DataAccess;
using UniversityManagement.Entities;

namespace UniversityManagement.Services;

public class GroupService
{
    private UniversityDbContext _dbContext;

    public GroupService(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool CreateGroup(Course selectedCourse, string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            MessageBox.Show("Please, enter a valid group name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        var groupNameExist = selectedCourse.Groups.Any(group => group.GroupName == groupName);

        if (groupNameExist)
        {
            MessageBox.Show("A group with the same name already exists. Please, use another name for the new group.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        var createNewGroup = new Group { GroupName = groupName };

        selectedCourse.Groups.Add(createNewGroup);

        _dbContext.Groups.Add(createNewGroup);

        _dbContext.SaveChanges();

        return true;
    }

    public bool DeleteGroup(Course selectedCourse, string groupName)
    {
        var deleteGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == groupName);

        if (deleteGroup == null) return false;

        if (deleteGroup.Students.Count > 0)
        {
            MessageBox.Show("Cannot delete this group because it contains students.\n" +
                            "If you want to remove a group, please remove the active students within it in " +
                            "\"Manage Students\" section before proceeding", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        foreach (var teacher in deleteGroup.GroupCurator)
        {
            teacher.CurrentGroupCurationName = null;
        }

        selectedCourse.Groups.Remove(deleteGroup);

        _dbContext.Groups.Remove(deleteGroup);

        _dbContext.SaveChanges();

        return true;
    }

    public bool EditGroupName(Course selectedCourse, string currentGroupName, string newGroupName)
    {
        var editGroupName = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == currentGroupName);

        if (editGroupName == null) return false;

        if (string.IsNullOrWhiteSpace(newGroupName))
        {
            MessageBox.Show("Please, enter a valid group name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        editGroupName.GroupName = newGroupName;

        var studentsToUpdate = _dbContext.Students.Where(s => s.CurrentGroupName == currentGroupName);

        foreach (var student in studentsToUpdate)
        {
            student.CurrentGroupName = newGroupName;
        }

        var teachersToUpdate = _dbContext.Teachers.Where(t => t.CurrentGroupCurationName == currentGroupName);

        foreach (var teacher in teachersToUpdate)
        {
            teacher.CurrentGroupCurationName = newGroupName;
        }

        _dbContext.SaveChanges();

        return true;
    }

    public bool SelectGroupCurator(Course selectedCourse, Teacher teacherToAssign, string groupName)
    {
        if (teacherToAssign == null || string.IsNullOrWhiteSpace(groupName))
        {
            MessageBox.Show("Please, select a both teacher and group from a list to apply curation", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        var selectedGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == groupName);

        if (selectedGroup == null) return false;

        selectedGroup.GroupCurator.Add(teacherToAssign);

        teacherToAssign.CurrentGroupCurationName = groupName;

        _dbContext.SaveChanges();

        return true;
    }

    // public bool CreateGroupInfoDocxFile(Course selectedCourse, string selectedGroupName, string filePath)
    // {
    //     var selectedGroup = _dbContext.Groups
    //         .Include(group => group.Students)
    //         .FirstOrDefault(group => group.CourseId == selectedCourse.CourseId && group.GroupName == selectedGroupName);
    //
    //     if (selectedGroup != null)
    //     {
    //         if (!selectedGroup.Students.Any())
    //         {
    //             MessageBox.Show("The selected group is empty. There are no students to include in the document", "Empty Group",
    //                 MessageBoxButton.OK, MessageBoxImage.Information);
    //
    //             return false;
    //         }
    //
    //         using var doc = DocX.Create(filePath);
    //
    //         doc.InsertParagraph(selectedCourse.CourseName).Bold().FontSize(18).Alignment = Alignment.center;
    //         doc.InsertParagraph(selectedGroup.GroupName).Bold().FontSize(14).Alignment = Alignment.center;
    //
    //         var students = selectedGroup.Students.Select(student => student.StudentFullName).ToList();
    //
    //         for (int i = 0; i < students.Count; i++)
    //         {
    //             doc.InsertParagraph($"{i + 1}. {students[i]}");
    //         }
    //
    //         doc.Save();
    //     }
    //
    //     return true;
    // }

    // public bool CreateGroupInfoPdfFile(Course selectedCourse, string selectedGroupName, string filePath)
    // {
    //     var selectedGroup = _dbContext.Groups
    //         .Include(group => group.Students)
    //         .FirstOrDefault(group => group.CourseId == selectedCourse.CourseId && group.GroupName == selectedGroupName);
    //
    //     if (selectedGroup != null)
    //     {
    //         if (!selectedGroup.Students.Any())
    //         {
    //             MessageBox.Show("The selected group is empty. There are no students to include in the document", "Empty Group",
    //                 MessageBoxButton.OK, MessageBoxImage.Information);
    //
    //             return false;
    //         }
    //
    //         var document = new PdfDocument();
    //         var page = document.AddPage();
    //         var gfx = XGraphics.FromPdfPage(page);
    //         
    //         var courseTitleFont = new XFont("Arial", 18, XFontStyle.Bold);
    //         var groupTitleFont = new XFont("Arial", 14, XFontStyle.Bold);
    //         var textFont = new XFont("Arial", 12, XFontStyle.Regular);
    //         
    //         gfx.DrawString(selectedCourse.CourseName, courseTitleFont, XBrushes.Black, new XRect(0, 40, page.Width, 0), XStringFormats.TopCenter);
    //         gfx.DrawString(selectedGroup.GroupName, groupTitleFont, XBrushes.Black, new XRect(0, 70, page.Width, 0), XStringFormats.TopCenter);
    //         
    //         var students = selectedGroup.Students.Select(student => student.StudentFullName).ToList();
    //         
    //         for (int i = 0; i < students.Count; i++)
    //         {
    //             byte[] utf8BytesConvert = Encoding.UTF8.GetBytes($"{i + 1}. {students[i]}");
    //             string utf8Text = Encoding.UTF8.GetString(utf8BytesConvert);
    //         
    //             gfx.DrawString(utf8Text, textFont, XBrushes.Black, new XRect(50, 100 + i * 20, page.Width - 100, 0));
    //         }
    //         
    //         document.Save(filePath);
    //     }
    //
    //     return true;
    // }
}