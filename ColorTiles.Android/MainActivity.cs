using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;

namespace ColorTiles.Android;

[Activity(
    Label = "Color Tiles",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
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

    private void SetFullScreen()
    {
        var sdkInt = (int)Build.VERSION.SdkInt;

#pragma warning disable CA1416, CA1422
        // Hide status bar on Android 11+
        if (sdkInt >= 30)
        {
            var windowsInsetsController = Window?.InsetsController;

            if (windowsInsetsController != null)
            {
                windowsInsetsController.SystemBarsBehavior = (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;

                windowsInsetsController.Hide(WindowInsets.Type.StatusBars());
            }
        }
        else if (sdkInt >= 21)
        {
            WindowCompat.SetDecorFitsSystemWindows(Window, false);

            StatusBarVisibility? systemVisibility = Window?.DecorView.SystemUiVisibility;

            if (systemVisibility != null)
            {
                var options = (int)systemVisibility;

                options |= (int)SystemUiFlags.Fullscreen;

                Window!.DecorView.SystemUiVisibility = (StatusBarVisibility)options;
            }

            var layoutNoLimitsFlag = WindowManagerFlags.LayoutNoLimits;
            Window?.SetFlags(layoutNoLimitsFlag, layoutNoLimitsFlag);
        }
#pragma warning restore CA1416, CA1422
    }
}
