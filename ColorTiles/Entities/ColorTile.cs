using System.Drawing;
using Avalonia.Media;

namespace ColorTiles.Entities
{
    public class ColorTile
    {
        public Point Position { get; set; }
        public IImage Image { get; set; }
        public int AtlasIndex { get; set; }

        public ColorTile(Point position, IImage image, int atlasIndex)
        {
            Position = position;
            Image = image;
            AtlasIndex = atlasIndex;
        }
    }
}