using System.Windows;
using UniversityManagement.Services;

namespace UniversityManagement.WPF.GroupManagementService;

public partial class GroupManagementWindow
{
    private GroupService _groupService;
    private DocxService _docxService;
    private PdfService _pdfService;

    public GroupManagementWindow(GroupService groupService, DocxService docxService, PdfService pdfService)
    {
        InitializeComponent();

        _groupService = groupService;
        _docxService = docxService;
        _pdfService = pdfService;
    }

    private void EditGroupInfo_Click(object sender, RoutedEventArgs e)
    {
        var editGroupInfoPage = new GroupManagementWindowEditGroupInfoPage(_groupService);

        EditGroupInfoButton.Style = (Style)FindResource("HighlightedButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = editGroupInfoPage;
    }

    private void CreateGroup_Click(object sender, RoutedEventArgs e)
    {
        var createGroupPage = new GroupManagementWindowCreateGroupPage(_groupService);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = createGroupPage;
    }

    private void DeleteGroup_Click(object sender, RoutedEventArgs e)
    {
        var deleteGroupPage = new GroupManagementWindowDeleteGroupPage(_groupService);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("HighlightedButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");

        MainFrame.Content = deleteGroupPage;
    }

    private void CreateFileWithGroup_Click(object sender, RoutedEventArgs e)
    {
        var createFileWithGroupPage =
            new GroupManagementWindowCreateFileWithGroupInfoPage(_groupService, _docxService, _pdfService);

        EditGroupInfoButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        DeleteGroupButton.Style = (Style)FindResource("NormalButtonStyle");
        CreateFileWithGroupInfoButton.Style = (Style)FindResource("HighlightedButtonStyle");

        MainFrame.Content = createFileWithGroupPage;
    }
}