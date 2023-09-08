using System;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ColorTiles.Entities.Tilesets;
using ColorTiles.Entities.Tools;
using ColorTiles.ViewModels.Controls;
using ColorTiles.ViewModels.Menus;
using ReactiveUI;

namespace ColorTiles.ViewModels;

public class MainViewModel : ViewModelBase
{
    private const string ASSET_LOCATION = "avares://ColorTiles/Assets";

    //public TileSetManager TileSetManager { get; set; }
    public ColorTileSet Tileset { get; set; }

    public GameBoardViewModel GameBoardViewModel { get; set; }
    public MainMenuViewModel MainMenuViewModel { get; set; }
    public GameOverMenuViewModel GameOverMenuViewModel { get; set; }

    private object _CurrentMenu;

    /// <summary>
    /// Gets or sets the current Menu
    /// </summary>
    public object CurrentMenu
    {
        get => _CurrentMenu;
        set => this.RaiseAndSetIfChanged(ref _CurrentMenu, value);
    }

    public MainViewModel()
    {
        //TileSetManager = new TileSetManager();
        Tileset = null!;

        Initialize();

        GameBoardViewModel = new GameBoardViewModel();
        MainMenuViewModel = new MainMenuViewModel(this);
        GameOverMenuViewModel = new GameOverMenuViewModel();

        // Navigate to MainMenu
        CurrentMenu = MainMenuViewModel;
    }

    private void Initialize()
    {
        if (!TryLoadImageAsset("tilesets/Color-Tiles.png", out IImage? image))
        {
            throw new Exception("Failed to load image asset.");
        }

        Tileset = new(0, image!);

        //TileSetManager.AddTileSet(tileset);
    }

    private bool TryLoadImageAsset(string path, out IImage? output)
    {
        output = null;

        try
        {
            output = new Bitmap(AssetLoader.Open(new Uri($"{ASSET_LOCATION}/{path}")));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
