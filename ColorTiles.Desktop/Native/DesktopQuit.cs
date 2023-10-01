using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ColorTiles.Interfaces;

namespace ColorTiles.Desktop.Native;

public class DesktopQuit : IPlatformQuit
{
    public void Quit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}