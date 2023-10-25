using System.Windows;
using UniversityManagement.DataAccess;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.GroupManagementService;

public partial class GroupManagementWindow
{
    private UniversityDbContext _dbContext;
    private GroupService _groupService;
    private DocxService _docxService;
    private PdfService _pdfService;

    public GroupManagementWindow(UniversityDbContext dbContext, GroupService groupService, DocxService docxService, PdfService pdfService)
    {
        InitializeComponent();

        _dbContext = dbContext;
        _groupService = groupService;
        _docxService = docxService;
        _pdfService = pdfService;
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
        var createFileWithGroupPage =
            new GroupManagementWindowCreateFileWithGroupInfoPage(_dbContext, _pdfService, _docxService);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = createFileWithGroupPage;
    }
}