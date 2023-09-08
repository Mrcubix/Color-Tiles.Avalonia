using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;

using Avalonia.Media;
using ColorTiles.Entities;
using ColorTiles.Entities.Tilesets;
using ReactiveUI;

namespace ColorTiles.ViewModels.Controls;

public class GameBoardViewModel : ViewModelBase
{
    private ColorTileSet? _tileset;
    private ObservableCollection<ColorTile> _colorTiles = new();
    private int _rows;
    private int _columns;

    public ColorTileSet? Tileset
    {
        get => _tileset;
        set => this.RaiseAndSetIfChanged(ref _tileset, value);
    }

    public ObservableCollection<ColorTile> ColorTiles
    {
        get => _colorTiles;
        set => this.RaiseAndSetIfChanged(ref _colorTiles, value);
    }

    public int Rows
    {
        get => _rows;
        set
        {
            this.RaiseAndSetIfChanged(ref _rows, value);
            DimensionsChanged?.Invoke(this, new Vector2(Columns, Rows));
        }
    }

    public int Columns
    {
        get => _columns;
        set
        {
            this.RaiseAndSetIfChanged(ref _columns, value);
            DimensionsChanged?.Invoke(this, new Vector2(Columns, Rows));
        }
    }

    public event EventHandler<Vector2>? DimensionsChanged;
    public event EventHandler? OnPenalty;
    public event EventHandler<int>? MatchesFound;

    public GameBoardViewModel()
    {
        Tileset = null!;

        Rows = 0;
        Columns = 0;

        DimensionsChanged += OnDimensionsChanged;
        OnPenalty = null!;
        MatchesFound = null!;

        InitializeBoard();
    }

    /// <summary>
    ///   Clears the board of all tiles.
    /// </summary>
    public void InitializeBoard()
    {
        ColorTiles.Clear();

        for (int i = 0; i < Rows * Columns; i++)
        {
            ColorTiles.Add(null!);
        }
    }

    #region Events

    public void OnBoardClicked(Avalonia.Point relativePosition)
    {
        // Convert the relative position to a tile index.
        Point position = new((int)(relativePosition.X / Tileset!.TilesetTileDimensions.Width), 
                             (int)(relativePosition.Y / Tileset.TilesetTileDimensions.Height));

        // If the tile index is out of bounds, return.
        if (position.X < 0 || position.X >= Rows || position.Y < 0 || position.Y >= Columns)
            return;

        // Get the tile at the index.
        ColorTile? tile = ColorTiles[position.Y * Rows + position.X];

        if (tile == null)
        {
            ColorTile?[] res = GetClosestAdjacentTiles(position);

            // check for matches
            int matchCount = RemoveMatches(res);

            if (matchCount == 0)
                OnPenalty?.Invoke(this, EventArgs.Empty);
            else
                MatchesFound?.Invoke(this, matchCount);
        }
    }

    public void OnDimensionsChanged(object? sender, Vector2 dimensions)
    {
        // Empty the board
    }

    #endregion

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
        if (position.X > 0)
        {
            currentX = position.X - 1;

            // we shift to the left until we find a tile
            do
            {
                adjacentTiles[0] = ColorTiles[ConvertToIndex(currentX--, position.Y)];
            } while (adjacentTiles[0] == null && currentX >= 0);
        }

        // right
        if (position.X < Columns - 1)
        {
            currentX = position.X + 1;

            // we shift to the right until we find a tile
            do
            {
                adjacentTiles[1] = ColorTiles[ConvertToIndex(currentX++, position.Y)];
            } while (adjacentTiles[1] == null && currentX < Columns);
        }

        // top
        if (position.Y > 0)
        {
            currentY = position.Y - 1;

            // we shift to the top until we find a tile
            do
            {
                adjacentTiles[2] = ColorTiles[ConvertToIndex(position.X, currentY--)];
            } while (adjacentTiles[2] == null && currentY >= 0);
        }

        // bottom
        if (position.Y < Rows - 1)
        {
            currentY = position.Y + 1;

            // we shift to the bottom until we find a tile
            do
            {
                adjacentTiles[3] = ColorTiles[ConvertToIndex(position.X, currentY++)];
            } while (adjacentTiles[3] == null && currentY < Rows);
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
            ColorTiles[ConvertToIndex(tile.Position)] = null!;


        return subjectForRemoval.Count;
    }

    #region Conversion

    private int ConvertToIndex(int x, int y) => y * Rows + x;
    private int ConvertToIndex(Point position) => ConvertToIndex(position.X, position.Y);

    #endregion
}
