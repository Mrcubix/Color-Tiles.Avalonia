using System.Drawing;

namespace ColorTiles.Entities
{
    public class ColorTile
    {
        public Point Position { get; set; }
        public int AtlasIndex { get; set; }

        public ColorTile(Point position, int atlasIndex)
        {
            Position = position;
            AtlasIndex = atlasIndex;
        }
    }
}