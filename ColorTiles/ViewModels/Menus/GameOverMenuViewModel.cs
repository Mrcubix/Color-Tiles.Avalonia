using System;
using Avalonia;
using Avalonia.Platform;
using ReactiveUI;

namespace ColorTiles.ViewModels.Menus;

public class GameOverMenuViewModel : ViewModelBase
{
    private int _score;
    private Thickness _padding;
    
    public int Score
    {
        get => _score;
        set => this.RaiseAndSetIfChanged(ref _score, value);
    }

    public Thickness Padding
    {
        get => _padding;
        set => this.RaiseAndSetIfChanged(ref _padding, value);
    }

    public event EventHandler? PlayAgainButtonClicked;
    public event EventHandler? QuitButtonClicked;

    public GameOverMenuViewModel() 
    { 
        Score = 0;

        if (OperatingSystem.IsAndroid() || OperatingSystem.IsBrowser())
            Padding = new Thickness(0, 4, 0, 0);
        else
            Padding = new Thickness(8, -6);
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
