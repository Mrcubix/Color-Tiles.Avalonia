using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using ColorTiles.Entities;
using ColorTiles.Entities.Tilesets;
using ReactiveUI;

namespace ColorTiles.ViewModels.Controls;

public class GameBoardViewModel : ToggleableControlViewModel
{
    private GameTileSet? _tileset;
    private ObservableCollection<ColorTile> _board = new();
    private int _columns;
    private int _rows;

    private int _tilesPerColor;
    private Avalonia.PixelSize _offset;
    private Avalonia.Size _zoom;

    private Random _random;

    /// <summary>
    ///   Tileset used to render the game board. <br/>
    ///   Tiles 0 to 8 are used for the game board. <br/>
    /// </summary>
    public GameTileSet? Tileset
    {
        get => _tileset;
        set
        {
            this.RaiseAndSetIfChanged(ref _tileset, value);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    public ObservableCollection<ColorTile> Board
    {
        get => _board;
        set
        {
            this.RaiseAndSetIfChanged(ref _board, value);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    ///   Numbers of columns in the game board. <br/>
    ///   Used for the array of <see cref="ColorTile"/>s and <br/>
    ///   the final size of the image representing the content of the game board.
    /// </summary>
    public int Columns
    {
        get => _columns;
        set
        {
            this.RaiseAndSetIfChanged(ref _columns, value);
            DimensionsChanged?.Invoke(this, new Avalonia.PixelSize(Columns, Rows));
        }
    }

    /// <summary>
    ///   Numbers of rows in the game board. <br/>
    ///   Used for the array of <see cref="ColorTile"/>s and <br/>
    ///   the final size of the image representing the content of the game board.
    /// </summary>
    public int Rows
    {
        get => _rows;
        set
        {
            this.RaiseAndSetIfChanged(ref _rows, value);
            DimensionsChanged?.Invoke(this, new Avalonia.PixelSize(Columns, Rows));
        }
    }

    /// <summary>
    ///   Number of tiles per color. <br/>
    /// </summary>
    public int TilesPerColor
    {
        get => _tilesPerColor;
        set
        {
            this.RaiseAndSetIfChanged(ref _tilesPerColor, value);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    ///   Offset of the game board. <br/>
    /// </summary>
    public Avalonia.PixelSize Offset
    {
        get => _offset;
        set
        {
            this.RaiseAndSetIfChanged(ref _offset, value);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    ///   Zoom of the game board. <br/>
    /// </summary>
    public Avalonia.Size Zoom
    {
        get => _zoom;
        set
        {
            this.RaiseAndSetIfChanged(ref _zoom, value);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public Avalonia.Size ScaledTileSize { get; set; }

    #region Events

    public event EventHandler? OnPenalty;
    public event EventHandler<int>? MatchesFound;

    public event EventHandler? BoardCleared;
    public event EventHandler? BoardGenerated;
    public event EventHandler<ColorTile?[]>? TilesRemovalRequested;

    public event EventHandler<Avalonia.PixelSize>? DimensionsChanged;
    public event EventHandler? SettingsChanged;

    #endregion

    #region Initialization

    public GameBoardViewModel(GameTileSet tileset, int columns, int rows, int tilesPerColor, Avalonia.PixelSize offset, Avalonia.Size zoom)
    {
        Tileset = tileset;

        if (Tileset != null)
            ScaledTileSize = new Avalonia.Size(Tileset.TextureResolution.Width, Tileset.TextureResolution.Height);

        Rows = rows;
        Columns = columns;

        TilesPerColor = tilesPerColor;

        Offset = offset;
        Zoom = zoom;

        Disable();

        _random = new Random();

        DimensionsChanged += OnDimensionsChanged;
        OnPenalty = null!;
        MatchesFound = null!;

        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public GameBoardViewModel(GameTileSet tileset, int columns, int rows, int tilesPerColor) : this(tileset, columns, rows, tilesPerColor, new Avalonia.PixelSize(3 * 48 + 16, 40), new Avalonia.Size(0.9, 0.9))
    {
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public GameBoardViewModel() : this(null!, 0, 0, 0)
    {
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///   Clears the board of all tiles.
    /// </summary>
    public void InitializeEmptyBoard()
    {
        _board.Clear();

        for (int i = 0; i < Rows * Columns; i++)
        {
            _board.Add(null!);
        }

        BoardCleared?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///   Generate the board with random tiles. <br/>
    ///   For each color in the atlas, generate <see cref="TilesPerColor"/> number of tiles.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///    The board is generated by generating a random position and checking if the position is empty. <br/>
    ///    If the position is empty, generate a random tile and place it on the board. <br/>
    ///  </para>
    /// </remarks>
    public void GenerateBoard()
    {
        if (Tileset == null)
            throw new Exception("Tileset is null.");

        // for every colors in the atlas (ColorTile.TILEMAP_WIDTH * ColorTile.TILEMAP_HEIGHT)
        for (int x = 0; x < Tileset.ColumnsOfPlayables; x++)
        {
            for (int y = 0; y < Tileset.RowsOfPlayables; y++)
            {
                var tilesToGenerate = _tilesPerColor;

                var tileIndex = Tileset.ConvertToIndex(x, y);

                if (tileIndex >= Tileset.BackgroundTileStartIndex)
                    continue;

                // while there are still tiles to generate
                while (tilesToGenerate > 0)
                {
                    var randomX = _random.Next(0, _columns);
                    var randomY = _random.Next(0, _rows);

                    var index = ConvertToIndex(randomX, randomY);

                    // if the position is empty
                    if (Board[index] == null)
                    {
                        // generate a random tile
                        Board[index] = new ColorTile(new Point(randomX, randomY), tileIndex);

                        // decrement the tiles to generate
                        tilesToGenerate--;
                    }
                }
            }
        }

        BoardGenerated?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Events

    /// <summary>
    ///   Called externally when the board is clicked.
    /// </summary>
    public void OnBoardClicked(Point relativePosition)
    {
        if (!IsEnabled)
            return;

        if (Tileset == null)
            throw new Exception("Tileset is null.");

        // Convert the relative position to a tile index.
        Point position = new((int)(relativePosition.X / ScaledTileSize.Width),
                             (int)(relativePosition.Y / ScaledTileSize.Height));

        // If the tile index is out of bounds, return.
        if (position.X < 0 || position.X >= Columns || position.Y < 0 || position.Y >= Rows)
            return;

#if DEBUG
        Console.WriteLine($"Clicked on tile {position.X}, {position.Y}");
#endif

        // Get the tile at the index.
        ColorTile? tile = _board[position.Y * Columns + position.X];

        if (tile == null)
        {
            ColorTile?[] res = GetClosestAdjacentTiles(position);

            TilesRemovalRequested?.Invoke(this, res);

            // check for matches
            int matchCount = RemoveMatches(res);

            if (matchCount == 0)
                OnPenalty?.Invoke(this, EventArgs.Empty);
            else
                MatchesFound?.Invoke(this, matchCount);
        }
    }

    public void OnDimensionsChanged(object? sender, Avalonia.PixelSize dimensions)
    {
        // Empty the board
        InitializeEmptyBoard();
    }

    #endregion

    #region Board manipulation

    /// <summary>
    /// Returns the closest adjacent 4 tiles to the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public ColorTile?[] GetClosestAdjacentTiles(Point position)
    {
        var adjacentTiles = new ColorTile?[4];

        int currentX;
        int currentY;

        // left
        if (position.X >= 0)
        {
            currentX = position.X - 1;

            // we shift to the left until we find a tile
            while (adjacentTiles[0] == null && currentX >= 0)
            {
                adjacentTiles[0] = _board[ConvertToIndex(currentX--, position.Y)];
            }
        }

        // right
        if (position.X < Columns)
        {
            currentX = position.X + 1;

            // we shift to the right until we find a tile
            while (adjacentTiles[1] == null && currentX < Columns)
            {
                adjacentTiles[1] = _board[ConvertToIndex(currentX++, position.Y)];
            }
        }

        // top
        if (position.Y >= 0)
        {
            currentY = position.Y - 1;

            // we shift to the top until we find a tile
            while (adjacentTiles[2] == null && currentY >= 0)
            {
                adjacentTiles[2] = _board[ConvertToIndex(position.X, currentY--)];
            }
        }

        // bottom
        if (position.Y < Rows)
        {
            currentY = position.Y + 1;

            // we shift to the bottom until we find a tile
            while (adjacentTiles[3] == null && currentY < Rows)
            {
                adjacentTiles[3] = _board[ConvertToIndex(position.X, currentY++)];
            }
        }

        return adjacentTiles;
    }

    /// <summary>
    /// If the crossing tiles are the same color, then they will be removed from the board.
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns>The number of matches removed.</returns>
    public int RemoveMatches(ColorTile?[] tiles)
    {
        List<ColorTile> subjectForRemoval = new();

        // check for matches
        foreach (var tile in tiles)
        {
            foreach (var otherTile in tiles)
            {
                if (tile == null || otherTile == null)
                    continue;

                if (tile == otherTile)
                    continue;

                if (subjectForRemoval.Contains(otherTile))
                    continue;

                if (tile.AtlasIndex == otherTile.AtlasIndex)
                {
                    subjectForRemoval.Add(otherTile);
                }
            }
        }

        // remove the subject tiles
        foreach (var tile in subjectForRemoval)
            _board[ConvertToIndex(tile.Position)] = null!;


        return subjectForRemoval.Count;
    }

    #endregion

    #region Tools

    public ColorTile? GetTileAt(Point position)
    {
        if (position.X < 0 || position.X >= Columns || position.Y < 0 || position.Y >= Rows)
            return null;

        return _board[ConvertToIndex(position)];
    }

    public ColorTile? GetTileAt(int x, int y)
    {
        if (x < 0 || x >= Columns || y < 0 || y >= Rows)
            return null;

        return _board[ConvertToIndex(x, y)];
    }

    #endregion

    #region Conversion

    private int ConvertToIndex(int x, int y) => y * Columns + x;
    private int ConvertToIndex(Point position) => ConvertToIndex(position.X, position.Y);

    #endregion
}
