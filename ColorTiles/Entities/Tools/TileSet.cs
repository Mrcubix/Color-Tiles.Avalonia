using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace ColorTiles.Entities.Tools
{
    public class TileSet : ReactiveObject, IDisposable
    {
        public int ID { get; set; }
        public IImage TilesetBitmap { get; set; }
        public List<IImage> Tiles { get; private set; }
        public PixelSize TilesetTileDimensions { get; set; }
        public PixelSize Separation { get; set; }
        public PixelSize TextureResolution { get; set; }

        public TileSet(int id, IImage tileSetBitmap, PixelSize separation, PixelSize textureResolution)
        {
            TilesetBitmap = tileSetBitmap;
            Separation = separation;
            TextureResolution = textureResolution;

            Tiles = new List<IImage>();
            TilesetTileDimensions = new PixelSize(
                (int)(TilesetBitmap.Size.Width / (TextureResolution.Width + Separation.Width)),
                (int)(TilesetBitmap.Size.Height / (TextureResolution.Height + Separation.Height))
            );

            Initialize();
        }

        public TileSet(int id, IImage tileSetBitmap, PixelSize textureResolution) : this(id, tileSetBitmap, new PixelSize(0, 0), textureResolution)
        {
        }

        public TileSet(int id, IImage tileSetBitmap) : this(id, tileSetBitmap, new PixelSize(0, 0), new PixelSize(1, 1))
        {
        }

        private void Initialize()
        {
            for (int y = 0; y < TilesetBitmap.Size.Height; y += (TextureResolution.Height + Separation.Height))
            {
                for (int x = 0; x < TilesetBitmap.Size.Width; x += (TextureResolution.Width + Separation.Width))
                {
                    ExtractTile(x, y);
                }
            }
        }

        private void ExtractTile(int x, int y)
        {
            Tiles.Add(
                new CroppedBitmap(TilesetBitmap, 
                    new PixelRect(x, y, TextureResolution.Width, TextureResolution.Height)));
        }

        public void Dispose()
        {
            foreach (var tile in Tiles)
            {
                (tile as IDisposable)?.Dispose();
            }

            Tiles.Clear();

            (TilesetBitmap as IDisposable)?.Dispose();
        }
    }
}