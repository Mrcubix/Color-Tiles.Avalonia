using System;
using System.Collections.Generic;
using System.Numerics;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ColorTiles.Entities.Tools
{
    public class TileSet
    {
        public int ID { get; set; }
        public IImage TileSetBitmap { get; set; }
        public List<IImage> Tiles { get; private set; }
        public Vector2 Separation { get; set; }
        public Vector2 TextureResolution { get; set; }

        public TileSet(int id, IImage tileSetBitmap, Vector2 separation, Vector2 textureResolution)
        {
            TileSetBitmap = tileSetBitmap;
            Separation = separation;
            TextureResolution = textureResolution;

            Tiles = new List<IImage>();

            Initialize();
        }

        public TileSet(int id, IImage tileSetBitmap, Vector2 textureResolution) : this(id, tileSetBitmap, new Vector2(0, 0), textureResolution)
        {
        }

        public TileSet(int id,IImage tileSetBitmap) : this(id, tileSetBitmap, new Vector2(0, 0), new Vector2(1, 1))
        {
        }

        private void Initialize()
        {
            // Seperate the tileset into individual tiles
            for (int y = 0; y < TileSetBitmap.Size.Height; y += (int) (TextureResolution.Y + Separation.Y))
            {
                for (int x = 0; x < TileSetBitmap.Size.Width; x += (int) (TextureResolution.X + Separation.X))
                {
                    ExtractTile(x, y);
                }
            }
        }

        private void ExtractTile(int x, int y)
        {
            Tiles.Add(
                new CroppedBitmap(TileSetBitmap, 
                    new PixelRect(x, y, (int)TextureResolution.X, (int)TextureResolution.Y)));
        }

        public void Dispose()
        {
            foreach (var tile in Tiles)
            {
                (tile as IDisposable)?.Dispose();
            }

            Tiles.Clear();

            (TileSetBitmap as IDisposable)?.Dispose();
        }

    }
}