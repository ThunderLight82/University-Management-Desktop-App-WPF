using System.Linq;
using DesktopApplication.Entities;
using System.Windows;

namespace DesktopApplication.GroupManagementService;

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

        _dbContext.SaveChanges();

        return true;
    }

    public bool SelectGroupCurator(Course selectedCourse, Teacher teacherToAssign, string groupName)
    {
        if (teacherToAssign == null || groupName == null)
        {
            MessageBox.Show("Please, select a both teacher and group from a list to apply curation", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        var selectedGroup = selectedCourse.Groups.FirstOrDefault(group => group.GroupName == groupName);

        if (selectedGroup == null) return false;

        selectedGroup.GroupCurator.Add(teacherToAssign);

        _dbContext.SaveChanges();

        return true;
    }

    public bool CreateFileWithGroupInfo(Course course, string groupName)
    {
        return true;
    }
}