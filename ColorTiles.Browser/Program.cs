using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.ReactiveUI;
using ColorTiles;
using ColorTiles.Browser.Entities.Audio.Files;
using ColorTiles.Entities.Audio.Sets;
using ColorTiles.Entities.Tools.Managers;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        await JSHost.ImportAsync("AudioFile.js", "./AudioFile.js");

        await BuildAvaloniaApp()
             .WithInterFont()
             .UseReactiveUI()
             .AfterSetup(AfterSetupCallBack)
             .StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>();

    public static void AfterSetupCallBack(AppBuilder builder)
    {
        if (Application.Current is not App app)
            return;

        // create the audio files
        var buttonClickSFX = new WebAudioFile(AudioSetManager.DefaultButtonClickSFXPath);
        var buttonHoverSFX = new WebAudioFile(AudioSetManager.DefaultButtonHoverSFXPath);
        var matchSFX = new WebAudioFile(AudioSetManager.DefaultMatchSFXPath);
        var penaltySFX = new WebAudioFile(AudioSetManager.DefaultPenaltySFXPath);
        //var gameOverSFX = new WebAudioFile(AudioSetManager.DefaultGameOverSFXPath);

        app.Audioset = new GameAudioSet(0, buttonClickSFX, buttonHoverSFX, matchSFX, penaltySFX, null!);
    }
}