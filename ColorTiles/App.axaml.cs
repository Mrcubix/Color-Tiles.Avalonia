using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ColorTiles.ViewModels;
using ColorTiles.Views;

namespace ColorTiles;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        //Assets.Localizations.Resources.Culture = CultureInfo.CurrentUICulture;

        // Check if culture is invariant
        if(CultureInfo.CurrentCulture == CultureInfo.InvariantCulture)
            Console.WriteLine("Invariant culture detected, changing localization is not supported.");
        else
            Assets.Localizations.Resources.Culture = new CultureInfo("en-US");

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}