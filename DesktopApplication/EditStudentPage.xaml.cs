using System.Windows;
using System.Windows.Controls;

namespace DesktopApplication;

public partial class EditStudentPage : Page
{
    private DataRepository _dataRepository;

    public EditStudentPage(DataRepository dataRepository) 
    { 
        InitializeComponent();
        _dataRepository = dataRepository;
    }

    private void AddStudent_Click(object sender, RoutedEventArgs e)
    {

    }

    private void DeleteStudent_Click(object sender, RoutedEvent e)
    {

    }
}