using System.Collections.Generic;
using System.Windows;

namespace DesktopApplication;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var courses = new List<Course>
        {
            new() {CourseId = 001, CourseName = "System Engineer"},
            new() {CourseId = 002, CourseName = "Software Engineer"},
            new() {CourseId = 003, CourseName = "Data Science"},
            new() {CourseId = 004, CourseName = "Data Analysis"},
            new() {CourseId = 005, CourseName = "Cyber Security"}
        };
        
        DataContext = courses;
    }
}