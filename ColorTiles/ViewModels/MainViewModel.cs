using System;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ColorTiles.Entities.Tilesets;
using ColorTiles.Entities.Tools;
using ColorTiles.ViewModels.Controls;
using ColorTiles.ViewModels.Menus;

namespace ColorTiles.ViewModels;

public class MainViewModel : ViewModelBase
{
    private const string ASSET_LOCATION = "avares://ColorTiles/Assets";

    //public TileSetManager TileSetManager { get; set; }
    public ColorTileSet Tileset { get; set; }

    public GameBoardViewModel GameBoardViewModel { get; set; }
    public MainMenuViewModel MainMenuViewModel { get; set; }
    public GameOverMenuViewModel GameOverMenuViewModel { get; set; }

    public MainViewModel()
    {
        //TileSetManager = new TileSetManager();
        Tileset = null!;

        Initialize();

        GameBoardViewModel = new GameBoardViewModel();
        MainMenuViewModel = new MainMenuViewModel();
        GameOverMenuViewModel = new GameOverMenuViewModel();
    }

    private void Initialize()
    {
        if (!TryLoadImageAsset("Tilesets/Color-Tiles.png", out IImage? image))
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
