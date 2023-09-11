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
            viewModel.GameOverMenuViewModel.QuitButtonClicked -= OnGameOverMenuQuitButtonClicked;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is MainViewModel viewModel)
        {
            viewModel.GameOverMenuViewModel.QuitButtonClicked += OnGameOverMenuQuitButtonClicked;
        }
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