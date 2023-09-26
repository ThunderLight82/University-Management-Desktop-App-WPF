﻿using System.Windows;

namespace DesktopApplication.Group_Management;

public partial class GroupManagementWindow
{
    private DataRepository _dataRepository;

    public GroupManagementWindow(DataRepository dataRepository)
    {
        InitializeComponent();
        _dataRepository = dataRepository;
    }

    private void EditGroupInfo_Click(object sender, RoutedEventArgs e)
    {
        var editGroupInfoPage = new GroupManagementWindowEditGroupInfoPage(_dataRepository);

        EditGroupInfoButton.Style = (Style)FindResource("HighlightedButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editGroupInfoPage;
    }

    private void CreateGroup_Click(object sender, RoutedEventArgs e)
    {
        var createGroupPage = new GroupManagementWindowCreateGroupPage(_dataRepository);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = createGroupPage;
    }

    private void DeleteGroup_Click(object sender, RoutedEventArgs e)
    {
        var deleteGroupPage = new GroupManagementWindowDeleteGroupPage(_dataRepository);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = deleteGroupPage;
    }

    private void CreateFileWithGroup_Click(object sender, RoutedEventArgs e)
    {
        var createFileWithGroupPage = new GroupManagementWindowCreateFileWithGroupInfoPage(_dataRepository);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = createFileWithGroupPage;
    }
}