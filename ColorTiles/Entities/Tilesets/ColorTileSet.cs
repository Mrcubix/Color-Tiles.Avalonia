using System;
using System.Numerics;
using Avalonia;
using Avalonia.Media;
using ColorTiles.Entities.Tools;

namespace ColorTiles.Entities.Tilesets;

public class ColorTileSet : TileSet
{
    private static readonly PixelSize Dimensions = new(3, 4);

    public IImage BackgroundTileLight { get; set; }
    public IImage BackgroundTileDark { get; set; }

    public ColorTileSet(int id, IImage image, PixelSize separation, PixelSize textureResolution) : base(id, image, separation, textureResolution)
    {
        if (Tiles.Count >= 12 && !IsBiggerThanDimensions(TilesetTileDimensions))
        {
            BackgroundTileLight = Tiles[9];
            BackgroundTileDark = Tiles[10];
        }
        else
        {
            throw new Exception("Tileset is not a valid Color Tiles TikleSet.");
        }
    }

    public ColorTileSet(int id, IImage image) : this(id, image, new PixelSize(0, 0), new PixelSize(48, 48))
    {
    }

    public IImage GetTile(int id) => Tiles[id];

    private bool IsBiggerThanDimensions(PixelSize vector) => vector.Width > Dimensions.Width && vector.Height > Dimensions.Height;
}