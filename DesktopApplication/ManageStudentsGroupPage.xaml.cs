using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class ManageStudentsGroupPage : Page
{
    private DataRepository _dataRepository;
    private HashSet<Student> _assignedStudents;

    public ManageStudentsGroupPage(DataRepository dataRepository, HashSet<Student> assignedStudents)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
        _assignedStudents = assignedStudents;
        GroupComboBox.ItemsSource = _dataRepository.Groups;
        StudentsListView.ItemsSource = _dataRepository.Students;
    }

    private void AddStudentsToGroup_Click(object sender, RoutedEventArgs e)
    {
        var selectedGroup = GroupComboBox.SelectedItem as Group;

        if (selectedGroup != null)
        {
            var selectedStudents = StudentsListView.SelectedItems.OfType<Student>().ToList();

            if (selectedStudents.Count > 0)
            {
                foreach (var student in selectedStudents)
                {
                    if (student.CurrentGroupName == null && !_assignedStudents.Contains(student))
                    {
                        student.CurrentGroupName = selectedGroup.GroupName;
                        selectedGroup.Students.Add(student);

                        _assignedStudents.Add(student);
                    }
                    else
                    {
                        MessageBox.Show("Student '" + student.StudentFullName + "' is already assigned to a group '" + student.CurrentGroupName + "'", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                StudentsListView.Items.Refresh();

                StudentsListView.SelectedItems.Clear();
            }
            else
            {
                MessageBox.Show("Please, select a student from the list to add to the group", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Please, select a group to add students to", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DeleteStudentsFromGroup_Click(object sender, RoutedEventArgs e)
    {
        var selectedStudents = StudentsListView.SelectedItems.OfType<Student>().ToList();

        if (selectedStudents.Count > 0)
        {
            foreach (var student in selectedStudents)
            {
                Group selectedGroup = _dataRepository.Groups.FirstOrDefault(group => group.Students.Contains(student));

                if (selectedGroup != null)
                {
                    selectedGroup.Students.Remove(student);
                    student.CurrentGroupName = null;

                    _assignedStudents.Remove(student);
                }
                else
                {
                    MessageBox.Show("This student is not assigned to any group", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            StudentsListView.Items.Refresh();

            StudentsListView.SelectedItems.Clear();
        }
        else
        {
            MessageBox.Show("Please, select a student from the list to remove it from the group", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

