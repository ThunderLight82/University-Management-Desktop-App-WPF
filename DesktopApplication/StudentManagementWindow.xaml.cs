using System.Windows;
using System.Collections.Generic;

namespace DesktopApplication;

public partial class StudentManagementWindow : Window
{
    private DataRepository _dataRepository;
    private HashSet<Student> _assignedStudents;

    public StudentManagementWindow(DataRepository dataRepository, HashSet<Student> assignedStudents)
    {
        _dataRepository = dataRepository;
        _assignedStudents = assignedStudents;
        InitializeComponent();
    }

    private void ManageStudentsGroupButton_Click(object sender, RoutedEventArgs e)
    {
        var manageStudentsGroupPage = new ManageStudentsGroupPage(_dataRepository, _assignedStudents);
        MainFrame.Content = manageStudentsGroupPage;
    }

    private void EditStudentsButton_Click(object sender, RoutedEventArgs e)
    {
        var manageStudentsGroupPage = new EditStudentPage(_dataRepository);
        MainFrame.Content = manageStudentsGroupPage;
    }

    private void ChangeStudentData_Click(object sender, RoutedEventArgs e)
    {

    }
}