﻿using System.Windows;
using UniversityManagement.Entities;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.TeacherManagementService;

public partial class TeacherManagementWindowEditTeacherPage
{
    private TeacherService _teacherService;

    public TeacherManagementWindowEditTeacherPage(TeacherService teacherService)
    {
        InitializeComponent();

        _teacherService = teacherService;

        TeachersListView.ItemsSource = _teacherService.PopulateTeacherList();
    }

    private void CreateTeacher_Click(object sender, RoutedEventArgs e)
    {
        string newTeacherFullName = NewTeacherFullNameTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(newTeacherFullName))
        {
            MessageBox.Show("Please, enter a valid teacher name", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return;
        }

        if (_teacherService.CheckIfTeacherExists(newTeacherFullName))
        {
            var duplicateTeacherQuestion = MessageBox.Show(
                "A teacher with the same name already exists. Do you want to add this teacher anyway?",
                "Duplication name",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (duplicateTeacherQuestion == MessageBoxResult.No)
            {
                return;
            }
        }

        _teacherService.CreateTeacher(newTeacherFullName);
        
        NewTeacherFullNameTextBox.Clear();

        TeachersListView.ItemsSource = _teacherService.PopulateTeacherList();
    }

    private void DeleteTeacher_Click(object sender, RoutedEventArgs e)
    {
        var selectedTeacher = TeachersListView.SelectedItem as Teacher;

        if (selectedTeacher == null)
        {
            MessageBox.Show("Please, select a teacher from the list below to remove", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);

            return;
        }

        _teacherService.DeleteTeacher(selectedTeacher);
        
        TeachersListView.ItemsSource = _teacherService.PopulateTeacherList();
    }
}