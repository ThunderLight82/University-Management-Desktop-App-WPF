using DesktopApplication.Entities;
using System.Linq;
using System.Windows;

namespace DesktopApplication;

public class TeacherService
{
    private UniversityDbContext _dbContext;

    public TeacherService (UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool CreateTeacher(string teacherFullName)
    {
        if (string.IsNullOrWhiteSpace(teacherFullName))
        {
            MessageBox.Show("Please, enter a valid teacher name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        var newTeacherNameAlreadyExists = _dbContext.Teachers.Any(teacher => 
            teacher.TeacherFullName == teacherFullName);

        if (newTeacherNameAlreadyExists)
        {
            var duplicateTeacherQuestion = MessageBox.Show(
                "A teacher with the same name already exists. Do you want to add this teacher anyway?",
                "Duplication name",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (duplicateTeacherQuestion == MessageBoxResult.No)
            {
                return false;
            }
        }

        var newTeacher = new Teacher
        {
            TeacherFullName = teacherFullName,
            IsCorrespondence = false
        };

        _dbContext.Teachers.Add(newTeacher);

        _dbContext.SaveChanges();

        return true;
    }

    public bool DeleteTeacher(Teacher selectedTeacher)
    {
        if (selectedTeacher == null)
        {
            MessageBox.Show("Please, select a teacher from the list below to remove", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        var associatedGroup = _dbContext.Groups.FirstOrDefault(group => group.GroupCurator.Contains(selectedTeacher));

        if (associatedGroup != null)
        {
            associatedGroup.GroupCurator.Remove(selectedTeacher);
        }

        selectedTeacher.CurrentGroupCurationName = "";

        _dbContext.Teachers.Remove(selectedTeacher);

        _dbContext.SaveChanges();

        return true;
    }

    public bool ChangeTeacherNameAndWorkInfo(Teacher selectedTeacher, string newFullName, bool isCorrespondence)
    {
        if (selectedTeacher == null)
        {
            MessageBox.Show("Please, select teacher from list first to update info", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        if (string.IsNullOrWhiteSpace(newFullName))
        {
            MessageBox.Show("Please, enter a valid teacher name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        selectedTeacher.TeacherFullName = newFullName;
        selectedTeacher.IsCorrespondence = isCorrespondence;

        _dbContext.SaveChanges();

        return true;
    }
}