using Avalonia.Controls;
using ColorTiles.ViewModels;

namespace ColorTiles.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (MainViewControl.DataContext is MainViewModel viewModel)
        {
            viewModel.Dispose();
        }

        base.OnClosing(e);
    }
}