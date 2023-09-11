using Avalonia.Controls;

namespace ColorTiles.Views.Controls;

public class ToggleableControl : UserControl
{
    public void Show()
    {
        IsVisible = true;
        IsEnabled = true;
    }

    public void Hide()
    {
        IsVisible = false;
        IsEnabled = false;
    }
}