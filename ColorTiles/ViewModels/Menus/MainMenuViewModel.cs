using System;
using ColorTiles.ViewModels.Controls;

namespace ColorTiles.ViewModels.Menus;

public class MainMenuViewModel : ToggleableControlViewModel
{
    public event EventHandler? PlayButtonClicked;

    public void OnPlayButtonClicked()
    {
        PlayButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
