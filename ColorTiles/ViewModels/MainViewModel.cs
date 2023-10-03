using System;
using ColorTiles.Entities.Tilesets;
using ColorTiles.Entities.Tools;
using ColorTiles.Entities.Tools.Managers;
using ColorTiles.ViewModels.Controls;
using ColorTiles.ViewModels.Menus;
using ReactiveUI;

namespace ColorTiles.ViewModels;

public class MainViewModel : ViewModelBase
{
    private HUDViewModel _hudViewModel = null!;
    private GameBoardViewModel _gameBoardViewModel = null!;
    private MainMenuViewModel _mainMenuViewModel = null!;
    private GameOverMenuViewModel _gameOverMenuViewModel = null!;

    private TileSetManager TilesetManager { get; set; } = null!;
    private AudioSetManager AudiosetManager { get; set; } = null!;

    public GameTileSet Tileset { get; set; }
    public AudioSet? Audioset { get; set; } = null!;

    public bool Disposed { get; private set; }

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
        TilesetManager = new TileSetManager();
        AudiosetManager = new AudioSetManager();

        Tileset = null!;
        Audioset = null!;

        Initialize();
        PostInitialize();
    }

    public MainViewModel(GameTileSet tileset, AudioSet audioset)
    {
        TilesetManager = new TileSetManager();
        AudiosetManager = new AudioSetManager();

        Tileset = tileset;
        Audioset = audioset;

        Initialize();
        PostInitialize();
    }

    protected virtual void Initialize()
    {
        Tileset ??= TilesetManager.LoadDefault();

        try
        {
            // All platforms except browsers can make use of OpenAL
            if (!OperatingSystem.IsBrowser())
                AudiosetManager.InitializeOpenAL();

            Audioset ??= AudiosetManager.LoadDefault();

            TweakAudio();
        }
        catch (Exception e)
        {
            Console.WriteLine("Audio failed to initialize with the following exception : ");
            Console.WriteLine(e);
            Console.WriteLine("Audio will be disabled.");

            Audioset = null!;
        }

        InitializeViewModels();
    }

    private void TweakAudio()
    {
        if (Audioset == null)
            return;

        Audioset.ButtonHoverSFX.Volume = 0.55f;
    }

    private void InitializeViewModels()
    {
        var canQuit = !(OperatingSystem.IsBrowser() || OperatingSystem.IsIOS());

        MainMenuViewModel = new MainMenuViewModel(Audioset);
        GameOverMenuViewModel = new GameOverMenuViewModel(Audioset, canQuit);

        HUDViewModel = new HUDViewModel(120, 10);
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

        HUDViewModel.ResetButtonClicked += OnPlayAgainButtonClicked;

        GameBoardViewModel.MatchesFound += HUDViewModel.OnMatchFround;
        GameBoardViewModel.MatchesFound += OnMatchFound;
        GameBoardViewModel.OnPenalty += OnPenalty;
        HUDViewModel.TimerBar.TimeExpired += OnTimeExpired;
    }

    #endregion

    #region Event handlers

    private void OnPlayButtonClicked(object? sender, EventArgs e)
    {
        Audioset?.ButtonClickSFX.Play();

        MainMenuViewModel.Disable();

        HUDViewModel.Score = 0;

        // Reset the game board
        GameBoardViewModel.Enable();
        GameBoardViewModel.InitializeEmptyBoard();
        GameBoardViewModel.GenerateBoard();

        // Show the HUD
        HUDViewModel.Enable();
        HUDViewModel.TimerBar.StartTimer();
    }

    private void OnPlayAgainButtonClicked(object? sender, EventArgs e)
    {
        // Play the button click SFX
        Audioset?.ButtonClickSFX.Play();

        // show the main menu
        MainMenuViewModel.Enable();

        // Disable the board
        GameBoardViewModel.Disable();
        // Reset the game board
        GameBoardViewModel.InitializeEmptyBoard();

        HUDViewModel.TimerBar.ResetTimer();
    }

    public void OnPenalty(object? sender, EventArgs e)
    {
        // Play the penalty SFX
        Audioset?.PenaltySFX?.Play();

        HUDViewModel.TimerBar.InflictPenalty(HUDViewModel.Penalty);

#if DEBUG
        Console.WriteLine($"Penalty inflicted: {HUDViewModel.Penalty}");
#endif
    }

    public void OnMatchFound(object? sender, int e)
    {
        // Play the match found SFX
        Audioset?.MatchSFX?.Play();
    }

    public void OnTimeExpired(object? sender, EventArgs e)
    {
        // Disable the board
        GameBoardViewModel.Disable();

        // play the game over SFX
        //Audioset?.GameOverSFX.Play();

        GameOverMenuViewModel.Score = HUDViewModel.Score;
        GameOverMenuViewModel.Enable();
    }

    #endregion

    #region Disposal

    public void Dispose()
    {
        if (Disposed)
            return;

        UnsuscribeFromEvents();

        // Dispose Image Side
        TilesetManager.Dispose();
        // Dispose Audio Side
        AudiosetManager.Dispose();

        GameBoardViewModel.Dispose();

        Disposed = true;
    }

    public void UnsuscribeFromEvents()
    {
        MainMenuViewModel.PlayButtonClicked -= OnPlayButtonClicked;

        GameOverMenuViewModel.PlayAgainButtonClicked -= OnPlayAgainButtonClicked;

        HUDViewModel.ResetButtonClicked -= OnPlayAgainButtonClicked;

        GameBoardViewModel.MatchesFound -= HUDViewModel.OnMatchFround;
        GameBoardViewModel.MatchesFound -= OnMatchFound;
        GameBoardViewModel.OnPenalty -= OnPenalty;
        HUDViewModel.TimerBar.TimeExpired -= OnTimeExpired;
    }

    #endregion
}