using System;

namespace ColorTiles.ViewModels.Menus;

public class MainMenuViewModel : ViewModelBase
{
    public event EventHandler? PlayButtonClicked;

    public void OnPlayButtonClicked()
    {
        PlayButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
