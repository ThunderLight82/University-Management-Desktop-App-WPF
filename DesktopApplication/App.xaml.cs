using System.Windows;

namespace DesktopApplication;

public partial class App : Application 
{
    public static DataRepository DataRepository { get; } = new DataRepository();
}