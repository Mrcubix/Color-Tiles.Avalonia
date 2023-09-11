using System;
using ColorTiles.ViewModels.Menus;
using ColorTiles.Views.Controls;

namespace ColorTiles.Views.Menus;

public partial class GameOverMenu : ToggleableControl
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