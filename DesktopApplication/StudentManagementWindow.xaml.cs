using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace DesktopApplication;

public partial class StudentManagementWindow : Window
{
    private DataRepository _dataRepository;

    public StudentManagementWindow(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository; 
        GroupComboBox.ItemsSource = _dataRepository.Groups;
        StudentsListView.ItemsSource = _dataRepository.Students;
    }

    private void AddStudentsToGroup_Click(object sender, RoutedEventArgs e)
    {
        Group selectedGroup = GroupComboBox.SelectedItem as Group;

        if (selectedGroup != null)
        {
            var selectedStudents = StudentsListView.SelectedItems.OfType<Student>().ToList();

            if (selectedStudents.Count > 0)
            {
                foreach (var student in selectedStudents)
                {
                    student.CurrentGroupName = selectedGroup.GroupName;
                    selectedGroup.Students.Add(student);
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
        Group selectedGroup = GroupComboBox.SelectedItem as Group;

        if (selectedGroup != null)
        {
            var selectedStudents = StudentsListView.SelectedItems.OfType<Student>().ToList();

            if (selectedStudents.Count > 0)
            {
                foreach (var student in selectedStudents)
                {
                    selectedGroup.Students.Remove(student);
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
        else
        {
            MessageBox.Show("Please, select a group to remove students from", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}