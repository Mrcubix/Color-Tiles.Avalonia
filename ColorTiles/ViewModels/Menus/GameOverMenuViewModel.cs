using System;
using Avalonia;
using ColorTiles.ViewModels.Controls;
using ReactiveUI;

namespace ColorTiles.ViewModels.Menus;

public class GameOverMenuViewModel : ToggleableControlViewModel
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

        if (!OperatingSystem.IsWindows())
            Padding = new Thickness(0, 4, 0, 0);
        else
            Padding = new Thickness(8, -6);

        Disable();
    }

    public GameOverMenuViewModel(int score) : this()
    {
        Score = score;
    }

    public void OnPlayAgainButtonClicked()
    {
        Disable();
        PlayAgainButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void OnQuitButtonClicked()
    {
        Disable();
        QuitButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
