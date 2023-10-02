using System;
using Avalonia.Input;
using ColorTiles.Entities.Tools;
using ColorTiles.ViewModels.Controls;

namespace ColorTiles.ViewModels.Menus;

public class MainMenuViewModel : ToggleableControlViewModel
{
    private readonly AudioSet? audioset;

    public event EventHandler? PlayButtonClicked;

    public MainMenuViewModel()
    {
    }

    public MainMenuViewModel(AudioSet? audioset)
    {
        this.audioset = audioset;
    }

    public void OnPlayButtonClicked()
    {
        PlayButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void OnButtonHovered()
    {
        audioset?.ButtonHoverSFX.Play();
    }
}
