using System;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ColorTiles.Entities.Tilesets;
using ColorTiles.ViewModels.Controls;
using ColorTiles.ViewModels.Menus;
using ReactiveUI;

namespace ColorTiles.ViewModels;

public class MainViewModel : ViewModelBase
{
    private const string ASSET_LOCATION = "avares://ColorTiles/Assets";

    private HUDViewModel _hudViewModel = null!;
    private GameBoardViewModel _gameBoardViewModel = null!;
    private MainMenuViewModel _mainMenuViewModel = null!;
    private GameOverMenuViewModel _gameOverMenuViewModel = null!;

    

    //public TileSetManager TileSetManager { get; set; }
    public GameTileSet Tileset { get; set; }

    public HUDViewModel HUDViewModel
    {
        get => _hudViewModel;
        set => this.RaiseAndSetIfChanged(ref _hudViewModel, value);
    }

    public GameBoardViewModel GameBoardViewModel
    {
        get => _gameBoardViewModel;
        set => this.RaiseAndSetIfChanged(ref _gameBoardViewModel, value);
    }

    public MainMenuViewModel MainMenuViewModel
    {
        get => _mainMenuViewModel;
        set => this.RaiseAndSetIfChanged(ref _mainMenuViewModel, value);
    }

    public GameOverMenuViewModel GameOverMenuViewModel
    {
        get => _gameOverMenuViewModel;
        set => this.RaiseAndSetIfChanged(ref _gameOverMenuViewModel, value);
    }

    #region Initialization

    public MainViewModel()
    {
        //TileSetManager = new TileSetManager();
        Tileset = null!;

        MainMenuViewModel = new MainMenuViewModel();
        GameOverMenuViewModel = new GameOverMenuViewModel();

        HUDViewModel = null!;
        GameBoardViewModel = null!;

        Initialize();
        PostInitialize();
    }

    protected virtual void Initialize()
    {
        LoadTileset("Tilesets/Color-Tiles.png");
        InitializeViewModels();
    }

    private void LoadTileset(string path)
    {
        // later on we might add support for custom tilesets
        if (!TryLoadImageAsset(path, out IImage? image))
        {
            throw new Exception("Failed to load image asset.");
        }

        Tileset = new DefaultTileSet(0, image!);
    }

    private void InitializeViewModels()
    {
        HUDViewModel = new HUDViewModel(120, 10000);
        GameBoardViewModel = new GameBoardViewModel(Tileset, 23, 15, 20);
    }

    protected virtual void PostInitialize()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        MainMenuViewModel.PlayButtonClicked += OnPlayButtonClicked;

        GameOverMenuViewModel.PlayAgainButtonClicked += OnPlayAgainButtonClicked;

        HUDViewModel.ResetButtonClicked += OnResetButtonClicked;

        GameBoardViewModel.MatchesFound += HUDViewModel.OnMatchFround;
        GameBoardViewModel.OnPenalty += OnPenalty;
        HUDViewModel.TimerBar.TimeExpired += OnTimeExpired;
    }

    #endregion

    #region Tools

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

    #endregion

    public bool ShowHUD()
    {
        if (!HUDViewModel.DoShowHUD)
        {
            HUDViewModel.DoShowHUD = true;
            return true;
        }

        return false;
    }

    public bool HideHUD()
    {
        if (HUDViewModel.DoShowHUD)
        {
            HUDViewModel.DoShowHUD = false;
            return true;
        }

        return false;
    }

    #region Event handlers

    private void OnPlayButtonClicked(object? sender, EventArgs e)
    {
        HUDViewModel.Score = 0;

        // Reset the game board
        GameBoardViewModel.InitializeEmptyBoard();
        GameBoardViewModel.GenerateBoard();

        // Show the HUD
        ShowHUD();
        HUDViewModel.TimerBar.StartTimer();
    }

    private void OnPlayAgainButtonClicked(object? sender, EventArgs e)
    {
        // Reset the game board
        GameBoardViewModel.InitializeEmptyBoard();

        HUDViewModel.TimerBar.ResetTimer();
    }

    private void OnResetButtonClicked(object? sender, EventArgs e)
    {
        GameBoardViewModel.InitializeEmptyBoard();

        HUDViewModel.TimerBar.ResetTimer();
    }

    public void OnPenalty(object? sender, EventArgs e)
    {
        HUDViewModel.TimerBar.InflictPenalty(HUDViewModel.Penalty);

#if DEBUG
        Console.WriteLine($"Penalty inflicted: {HUDViewModel.Penalty}");
#endif
    }

    public void OnTimeExpired(object? sender, EventArgs e)
    {
        GameOverMenuViewModel.Score = HUDViewModel.Score;
    }

    #endregion
}
