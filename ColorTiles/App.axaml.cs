using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ColorTiles.Entities.Tilesets;
using ColorTiles.Entities.Tools;
using ColorTiles.ViewModels;
using ColorTiles.Views;

namespace ColorTiles;

public partial class App : Application
{
    public GameTileSet Tileset { get; set; }
    public AudioSet Audioset { get; set; }

    public App()
    {
        Tileset = null!;
        Audioset = null!;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Check if culture is invariant
        if (CultureInfo.CurrentCulture == CultureInfo.InvariantCulture)
            Console.WriteLine("Invariant culture detected, changing localization is not supported.");
        else
            Assets.Localizations.Resources.Culture = new CultureInfo("en-US");

        // Later on, check settings from file to see if a custom tileset and audioset should be loaded (if not a browser launch)

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(Tileset, Audioset)
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel(Tileset, Audioset)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}