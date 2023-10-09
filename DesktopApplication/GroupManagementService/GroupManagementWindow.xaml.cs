using System.Windows;

namespace DesktopApplication.GroupManagementService;

public partial class GroupManagementWindow
{
    private UniversityDbContext _dbContext;
    private GroupService _groupService;

    public GroupManagementWindow(UniversityDbContext dbContext, GroupService groupService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _groupService = groupService;
    }

    private void EditGroupInfo_Click(object sender, RoutedEventArgs e)
    {
        var editGroupInfoPage = new GroupManagementWindowEditGroupInfoPage(_dbContext, _groupService);

        EditGroupInfoButton.Style = (Style)FindResource("HighlightedButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editGroupInfoPage;
    }

    private void CreateGroup_Click(object sender, RoutedEventArgs e)
    {
        var createGroupPage = new GroupManagementWindowCreateGroupPage(_dbContext, _groupService);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = createGroupPage;
    }

    private void DeleteGroup_Click(object sender, RoutedEventArgs e)
    {
        var deleteGroupPage = new GroupManagementWindowDeleteGroupPage(_dbContext, _groupService);

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