using System;
using Avalonia.Controls;
using ColorTiles.ViewModels;
using ColorTiles.ViewModels.Menus;

namespace ColorTiles.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is MainViewModel viewModel)
        {
            
        }
    }

    private void OnMainMenuDataContextChanged(object? sender, EventArgs e)
    {
        if (MainMenuControl.DataContext is MainMenuViewModel viewModel)
        {
            viewModel.PlayButtonClicked += OnMainMenuPlayButtonClicked;
        }
    }

    private void OnGameOverMenuDataContextChanged(object? sender, EventArgs e)
    {
        if (GameOverMenuControl.DataContext is GameOverMenuViewModel viewModel)
        {
            viewModel.PlayAgainButtonClicked += OnGameOverMenuPlayAgainButtonClicked;
            viewModel.QuitButtonClicked += OnGameOverMenuQuitButtonClicked;
        }
    }

    private void OnMainMenuPlayButtonClicked(object? sender, EventArgs e)
    {
        MainMenuControl.Hide();
        GameOverMenuControl.Hide();
    }

    private void OnGameOverMenuPlayAgainButtonClicked(object? sender, EventArgs e)
    {
        MainMenuControl.Show();
        GameOverMenuControl.Hide();
    }

    private void OnGameOverMenuQuitButtonClicked(object? sender, EventArgs e)
    {
        Environment.Exit(0);
    }
}