using Avalonia;
using Avalonia.Media;

namespace ColorTiles.Entities.Tilesets;

public class DefaultTileSet : GameTileSet
{
    public DefaultTileSet(int id, IImage image, PixelSize separation, PixelSize textureResolution) : base(id, image, separation, textureResolution)
    {
        Dimensions = new PixelSize(3, 4);

        ColumnsOfPlayables = 4;
        RowsOfPlayables = 3;
        PlayableTiles = 10;

        lightBackgroundTileIndex = 10;
        darkBackgroundTileIndex = 11;

        Initialize();
    }

    public DefaultTileSet(int id, IImage image) : this(id, image, new PixelSize(0, 0), new PixelSize(48, 48))
    {
    }
}