using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ColorTiles.ViewModels;
using ColorTiles.Views.Controls;

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
        if (DataContext is MainViewModel viewModel)
        {
            viewModel.Dispose();
        }
            
        // TODO: might want to use DI to provide a proper implementation of quitting native to each platforms without classic desktop lifetime
        //_window?.Close();
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
        else if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            
        }
    }
}