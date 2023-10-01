using System;
using Avalonia.Android;
using ColorTiles.Interfaces;

namespace ColorTiles.Android;

public class AndroidQuit : IPlatformQuit
{
    private readonly AvaloniaMainActivity _mainActivity;

    public AndroidQuit(AvaloniaMainActivity mainActivity) => _mainActivity = mainActivity;

    public void Quit()
    {
        // FinishAndRemoveTask() is not enough to exit the app. Is this Intentional?
        _mainActivity.FinishAndRemoveTask();
        Environment.Exit(0);
    }
}