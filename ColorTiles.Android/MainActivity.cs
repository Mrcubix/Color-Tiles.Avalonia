using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using ColorTiles.Interfaces;
using Splat;

namespace ColorTiles.Android;

[Activity(
    Label = "Color Tiles",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    internal readonly AndroidQuit platformQuit = null!;

    public MainActivity() => platformQuit = new AndroidQuit(this);

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        Locator.CurrentMutable.RegisterConstant<IPlatformQuit>(platformQuit);

        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        // Force landscape orientation
        RequestedOrientation = ScreenOrientation.SensorLandscape;
        // Make app fullscreen
        Window?.AddFlags(WindowManagerFlags.Fullscreen);
        // Hide keyboard
        Window?.SetSoftInputMode(SoftInput.StateAlwaysHidden);

        SetFullScreen();

        base.OnCreate(savedInstanceState);
    }

    public override void OnBackPressed()
    {
        base.OnBackPressed();

        // Might remove that later
        platformQuit.Quit();
    }

    private void SetFullScreen()
    {
        var sdkInt = (int)Build.VERSION.SdkInt;

#pragma warning disable CA1416, CA1422
        // Hide status bar on Android 11+
        if (sdkInt >= 30)
        {
            /*var windowsInsetsController = Window?.InsetsController;

            if (windowsInsetsController != null)
            {
                windowsInsetsController.SystemBarsBehavior = (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;

                windowsInsetsController.Hide(WindowInsets.Type.StatusBars());
            }*/
        }
        else if (sdkInt >= 21)
        {
            StatusBarVisibility? systemVisibility = Window!.DecorView.SystemUiVisibility;

            var options = (int)systemVisibility;

            options |= (int)SystemUiFlags.Fullscreen;
            options |= (int)SystemUiFlags.LayoutStable;

            Window!.DecorView.SystemUiVisibility = (StatusBarVisibility)options;

            Window?.AddFlags(WindowManagerFlags.LayoutNoLimits);
        }
#pragma warning restore CA1416, CA1422
    }
}
