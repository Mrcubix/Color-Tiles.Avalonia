using System;
using System.Drawing;

namespace ColorTiles.ViewModels.Menus;

public class MainMenuViewModel : ViewModelBase
{
    private Size _baseSize = new Size(1280, 720);
    private Size _baseTitleSize = new(594, 115);
    private Size _basePlaySize = new(314, 112);
    private Size _titleSize = new(594, 115);

    public event EventHandler? PlayButtonClicked;

    public void OnPlayButtonClicked()
    {
        PlayButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
