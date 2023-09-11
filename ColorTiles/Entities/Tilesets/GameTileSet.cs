using System;
using Avalonia;
using Avalonia.Media;
using ColorTiles.Entities.Tools;
using ReactiveUI;

namespace ColorTiles.Entities.Tilesets;

public abstract class GameTileSet : TileSet
{
    protected int _rowsOfPlayables;
    protected int _columnsOfPlayables;
    protected int _playableTiles;

    protected int lightBackgroundTileIndex = 9;
    protected int darkBackgroundTileIndex = 10;

    protected IImage _backgroundTileLight;
    protected IImage _backgroundTileDark;

    protected PixelSize Dimensions { get; init; } 

    #region Playable tiles

    public int ColumnsOfPlayables
    {
        get => _columnsOfPlayables;
        set => this.RaiseAndSetIfChanged(ref _columnsOfPlayables, value);
    }

    public int RowsOfPlayables
    {
        get => _rowsOfPlayables;
        set => this.RaiseAndSetIfChanged(ref _rowsOfPlayables, value);
    }

    public int PlayableTiles
    {
        get => _playableTiles;
        set => this.RaiseAndSetIfChanged(ref _playableTiles, value);
    }

    public int BackgroundTileStartIndex => Math.Min(lightBackgroundTileIndex, darkBackgroundTileIndex);

    #endregion

    public GameTileSet(int id, IImage image, PixelSize separation, PixelSize textureResolution) :  base(id, image, separation, textureResolution)
    {
        _backgroundTileDark = null!;
        _backgroundTileLight = null!;
    }

    public virtual void Initialize()
    {
        if (Tiles.Count >= Math.Max(lightBackgroundTileIndex, darkBackgroundTileIndex) && !IsLargerThanDimensions(TilesetTileDimensions))
        {
            _backgroundTileLight = Tiles[lightBackgroundTileIndex];
            _backgroundTileDark = Tiles[darkBackgroundTileIndex];
        }
        else
        {
            throw new Exception("Tileset is not a valid Color Tiles TileSet.");
        }
    }

    #region Background tiles

    public IImage BackgroundTileLight
    {
        get => _backgroundTileLight;
        set => this.RaiseAndSetIfChanged(ref _backgroundTileLight, value);
    }

    public IImage BackgroundTileDark
    {
        get => _backgroundTileDark;
        set => this.RaiseAndSetIfChanged(ref _backgroundTileDark, value);
    }

    #endregion

    public IImage GetTile(int id) => Tiles[id];
    public IImage GetTile(int x, int y) => Tiles[y * ColumnsOfPlayables + x];

    protected bool IsLargerThanDimensions(PixelSize vector) => vector.Width > Dimensions.Width && vector.Height > Dimensions.Height;

    public int ConvertToIndex(int x, int y) => Math.Clamp(y * ColumnsOfPlayables + x, 0, Tiles.Count - 1);
    public int ConvertToIndex(Point point) => ConvertToIndex((int)point.X, (int)point.Y);
}