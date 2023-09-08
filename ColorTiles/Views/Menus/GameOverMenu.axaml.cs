using System;
using Avalonia.Controls;
using ColorTiles.ViewModels.Menus;

namespace ColorTiles.Views.Menus;

public partial class GameOverMenu : UserControl
{
    public event EventHandler? PlayAgainButtonClicked;
    public event EventHandler? QuitButtonClicked;

    public GameOverMenu()
    {
        InitializeComponent();

        Hide();
    }

    protected override void OnDataContextBeginUpdate()
    {
        base.OnDataContextBeginUpdate();

        if (DataContext is GameOverMenuViewModel viewModel)
        {
            viewModel.PlayAgainButtonClicked -= OnPlayAgainButtonClicked;
            viewModel.QuitButtonClicked -= OnQuitButtonClicked;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is GameOverMenuViewModel viewModel)
        {
            viewModel.PlayAgainButtonClicked += OnPlayAgainButtonClicked;
            viewModel.QuitButtonClicked += OnQuitButtonClicked;
        }
    }

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

    #region Event Handlers

    private void OnPlayAgainButtonClicked(object? sender, EventArgs e)
    {
        Hide();

        PlayAgainButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnQuitButtonClicked(object? sender, EventArgs e)
    {
        Hide();

        QuitButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}