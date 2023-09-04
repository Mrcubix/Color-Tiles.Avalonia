using Avalonia.Media;

namespace ColorTiles.Interfaces
{
    public interface IGameAssets
    {
        public IImage TitleImage { get; set; }
        public IImage PlayButton { get; set; }
        public IImage TileSet { get; set; }
    }
}