using System.Windows;

namespace DesktopApplication.Group_Management;

public partial class GroupManagementWindow
{
    private UniversityDbContext _dbContext;

    public GroupManagementWindow(UniversityDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext;
    }

    private void EditGroupInfo_Click(object sender, RoutedEventArgs e)
    {
        var editGroupInfoPage = new GroupManagementWindowEditGroupInfoPage(_dbContext);

        EditGroupInfoButton.Style = (Style)FindResource("HighlightedButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editGroupInfoPage;
    }

    private void CreateGroup_Click(object sender, RoutedEventArgs e)
    {
        var createGroupPage = new GroupManagementWindowCreateGroupPage(_dbContext);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = createGroupPage;
    }

    private void DeleteGroup_Click(object sender, RoutedEventArgs e)
    {
        var deleteGroupPage = new GroupManagementWindowDeleteGroupPage(_dbContext);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = deleteGroupPage;
    }

    private void CreateFileWithGroup_Click(object sender, RoutedEventArgs e)
    {
        var createFileWithGroupPage = new GroupManagementWindowCreateFileWithGroupInfoPage(_dbContext);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = createFileWithGroupPage;
    }
}