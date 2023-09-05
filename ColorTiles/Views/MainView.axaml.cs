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

    protected override void OnDataContextBeginUpdate()
    {
        base.OnDataContextBeginUpdate();

        if (DataContext is MainViewModel viewModel)
        {
            MainMenuControl.DataContextChanged -= OnMainMenuDataContextChanged;
            GameOverMenuControl.DataContextChanged -= OnGameOverMenuDataContextChanged;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is MainViewModel viewModel)
        {
            MainMenuControl.DataContextChanged += OnMainMenuDataContextChanged;
            GameOverMenuControl.DataContextChanged += OnGameOverMenuDataContextChanged;
        }
    }

    /// <summary>
    ///    This method is called when the DataContext of the MainMenu changes.
    /// </summary>
    private void OnMainMenuDataContextChanged(object? sender, EventArgs e)
    {
        if (MainMenuControl.DataContext is MainMenuViewModel viewModel)
        {
            viewModel.PlayButtonClicked += OnMainMenuPlayButtonClicked;
        }
    }

    /// <summary>
    ///   This method is called when the DataContext of the GameOverMenu changes.
    /// </summary>
    private void OnGameOverMenuDataContextChanged(object? sender, EventArgs e)
    {
        if (GameOverMenuControl.DataContext is GameOverMenuViewModel viewModel)
        {
            viewModel.PlayAgainButtonClicked += OnGameOverMenuPlayAgainButtonClicked;
            viewModel.QuitButtonClicked += OnGameOverMenuQuitButtonClicked;
        }
    }

    /// <summary>
    ///  This method is called when the Play button is clicked on the MainMenu. <br/>
    ///  
    /// </summary>
    private void OnMainMenuPlayButtonClicked(object? sender, EventArgs e)
    {
        
    }

    /// <summary>
    ///  This method is called when the Play Again button is clicked on the GameOverMenu. <br/>
    ///  This method shows the MainMenu.
    /// </summary>
    private void OnGameOverMenuPlayAgainButtonClicked(object? sender, EventArgs e)
    {
        MainMenuControl.Show();
    }

    /// <summary>
    ///   This method is called when the Quit button is clicked on the GameOverMenu. <br/>
    ///   This method exits the application.
    /// </summary>
    private void OnGameOverMenuQuitButtonClicked(object? sender, EventArgs e)
    {
        Environment.Exit(0);
    }
}