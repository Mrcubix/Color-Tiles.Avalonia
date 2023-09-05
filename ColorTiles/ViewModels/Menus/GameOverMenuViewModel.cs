using System;

namespace ColorTiles.ViewModels.Menus;

public class GameOverMenuViewModel : ViewModelBase
{
    public int Score { get; set; }

    public event EventHandler? PlayAgainButtonClicked;
    public event EventHandler? QuitButtonClicked;

    public GameOverMenuViewModel() 
    { 
        Score = 0;
    }

    public GameOverMenuViewModel(int score)
    {
        Score = score;
    }

    public void OnPlayAgainButtonClicked()
    {
        PlayAgainButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void OnQuitButtonClicked()
    {
        QuitButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
