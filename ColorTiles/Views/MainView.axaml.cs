using System;
using Avalonia.Controls;
using ColorTiles.Interfaces;
using ColorTiles.ViewModels;
using Splat;

namespace ColorTiles.Views;

public partial class MainView : UserControl
{
    private Window? _window;

    public MainView()
    {
        InitializeComponent();

        Loaded += (_, _) =>
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
            viewModel.GameOverMenuViewModel.QuitButtonClicked -= OnGameOverMenuQuitButtonClicked;
            GameBoardControl.OffsetChanged -= viewModel.HUDViewModel.OnOffsetChanged;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is MainViewModel viewModel)
        {
            viewModel.GameOverMenuViewModel.QuitButtonClicked += OnGameOverMenuQuitButtonClicked;
            GameBoardControl.OffsetChanged += viewModel.HUDViewModel.OnOffsetChanged;
        }
    }

    /// <summary>
    ///   This method is called when the Quit button is clicked on the GameOverMenu. <br/>
    ///   This method exits the application.
    /// </summary>
    private void OnGameOverMenuQuitButtonClicked(object? sender, EventArgs e)
    {
        // Obtain currently binded IPlatformQuit
        var platformQuit = Locator.Current.GetService<IPlatformQuit>();

        if (platformQuit != null)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.Dispose();
            }

            platformQuit.Quit();
        }
    }
}