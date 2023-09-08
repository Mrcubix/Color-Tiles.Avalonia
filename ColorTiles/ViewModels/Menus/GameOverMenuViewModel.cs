using System;
using ReactiveUI;

namespace ColorTiles.ViewModels.Menus;

public class GameOverMenuViewModel : ViewModelBase
{
    private int _score;
    
    public int Score
    {
        get => _score;
        set => this.RaiseAndSetIfChanged(ref _score, value);
    }

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
