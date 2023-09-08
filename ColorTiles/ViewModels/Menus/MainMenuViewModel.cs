using System;

namespace ColorTiles.ViewModels.Menus;

public class MainMenuViewModel : ViewModelBase
{

    private readonly MainViewModel _Parent;

    public MainMenuViewModel(MainViewModel parent)
    {
        _Parent = parent;
    }

    public void PlayGameCommand()
    {
        _Parent.CurrentMenu = _Parent.GameOverMenuViewModel;
    }
}
