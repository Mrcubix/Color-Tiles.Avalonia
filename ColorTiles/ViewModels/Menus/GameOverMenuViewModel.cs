﻿using System;
using Avalonia;
using ColorTiles.Entities.Tools;
using ColorTiles.ViewModels.Controls;
using ReactiveUI;

namespace ColorTiles.ViewModels.Menus;

public class GameOverMenuViewModel : ToggleableControlViewModel
{
    private readonly AudioSet? audioset;

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

    public bool CanQuit { get; init; }

    public event EventHandler? PlayAgainButtonClicked;
    public event EventHandler? QuitButtonClicked;

    public GameOverMenuViewModel(bool canQuit = true)
    { 
        Score = 0;

        if (!OperatingSystem.IsWindows())
            Padding = new Thickness(0, 4, 0, 0);
        else
            Padding = new Thickness(8, -6);

        CanQuit = canQuit;

        Disable();
    }

    public GameOverMenuViewModel(int score) : this()
    {
        Score = score;
    }

    public GameOverMenuViewModel(AudioSet? audioset, bool canQuit = true) : this(canQuit)
    {
        this.audioset = audioset;
    }

    public void OnPlayAgainButtonClicked()
    {
        Disable();
        PlayAgainButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void OnQuitButtonClicked()
    {
        QuitButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void OnButtonHovered()
    {
        audioset?.ButtonHoverSFX.Play();
    }
}
