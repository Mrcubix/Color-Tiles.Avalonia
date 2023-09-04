
using System;
using Avalonia.Media;
using ColorTiles.Entities.Tools;

namespace ColorTiles.ViewModels.Menus;

public class GameOverMenuViewModel : ViewModelBase
{
    public int Score { get; set; }

    public string ScoreLabel { get; set; } = string.Empty;
    public string PlayAgainLabel { get; set; } = string.Empty;
    public string QuitLabel { get; set; } = string.Empty;

    public event EventHandler? PlayAgainButtonClicked;
    public event EventHandler? QuitButtonClicked;

    public GameOverMenuViewModel() 
    { 
        Score = 0;

        SetLocalization(Localization.Default);
    }

    public GameOverMenuViewModel(int score)
    {
        Score = score;

        SetLocalization(Localization.Default);
    }

    public GameOverMenuViewModel(int score, GameStrings strings) : this(score)
    {
        ScoreLabel = strings.ScoreLabel;
        PlayAgainLabel = strings.PlayAgainLabel;
        QuitLabel = strings.QuitLabel;
    }

    public GameOverMenuViewModel(int score, Localization localization) : this(score)
    {
        var strings = localization.Strings;

        ScoreLabel = strings.ScoreLabel;
        PlayAgainLabel = strings.PlayAgainLabel;
        QuitLabel = strings.QuitLabel;
    }

    public GameOverMenuViewModel(int score, string scoreLabel, string playAgainLabel, string quitLabel) : this(score)
    {
        ScoreLabel = scoreLabel;
        PlayAgainLabel = playAgainLabel;
        QuitLabel = quitLabel;
    }

    public void SetLocalization(Localization localization)
    {
        var strings = localization.Strings;

        ScoreLabel = strings.ScoreLabel;
        PlayAgainLabel = strings.PlayAgainLabel;
        QuitLabel = strings.QuitLabel;
    }

    public void SetStrings(GameStrings strings)
    {
        ScoreLabel = strings.ScoreLabel;
        PlayAgainLabel = strings.PlayAgainLabel;
        QuitLabel = strings.QuitLabel;
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
