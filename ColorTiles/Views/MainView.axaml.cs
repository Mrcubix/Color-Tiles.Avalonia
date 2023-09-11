using System;
using Avalonia.Controls;
using ColorTiles.ViewModels;

namespace ColorTiles.Views;

public partial class MainView : UserControl
{
    private Window? _window;

    public MainView()
    {
        InitializeComponent();

        Loaded += (_,_) => 
        {
            _window = TopLevel.GetTopLevel(this) as Window;
        };

        Unloaded += (_, _) =>
        {
            _window = null;
        };
    }

    protected override void OnDataContextBeginUpdate()
    {
        base.OnDataContextBeginUpdate();

        if (DataContext is MainViewModel viewModel)
        {
            viewModel.GameOverMenuViewModel.PlayAgainButtonClicked -= OnReset;
            viewModel.GameOverMenuViewModel.QuitButtonClicked -= OnGameOverMenuQuitButtonClicked;

            viewModel.HUDViewModel.ResetButtonClicked -= OnReset;
            viewModel.HUDViewModel.TimerBar.TimeExpired -= OnTimeExpired;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is MainViewModel viewModel)
        {
            viewModel.GameOverMenuViewModel.PlayAgainButtonClicked += OnReset;
            viewModel.GameOverMenuViewModel.QuitButtonClicked += OnGameOverMenuQuitButtonClicked;

            viewModel.HUDViewModel.ResetButtonClicked += OnReset;
            viewModel.HUDViewModel.TimerBar.TimeExpired += OnTimeExpired;
        }
    }

    /// <summary>
    ///  This method is called when the Play Again button is clicked on the GameOverMenu. <br/>
    ///  This method shows the MainMenu.
    /// </summary>
    private void OnReset(object? sender, EventArgs e)
    {
        MainMenuControl.Show();
    }

    private void OnTimeExpired(object? sender, EventArgs e)
    {
        GameOverMenuControl.Show();
    }

    /// <summary>
    ///   This method is called when the Quit button is clicked on the GameOverMenu. <br/>
    ///   This method exits the application.
    /// </summary>
    private void OnGameOverMenuQuitButtonClicked(object? sender, EventArgs e)
    {
        _window?.Close();
    }
}