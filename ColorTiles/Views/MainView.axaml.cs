using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using ColorTiles.ViewModels;
using ColorTiles.ViewModels.Menus;

namespace ColorTiles.Views;

public partial class MainView : UserControl
{
    private Window? _window;

    public MainView()
    {
        InitializeComponent();

        _window = TopLevel.GetTopLevel(this) as Window;
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