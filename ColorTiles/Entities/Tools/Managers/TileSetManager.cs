using System;
using System.Collections.Generic;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ColorTiles.Entities.Tilesets;

namespace ColorTiles.Entities.Tools.Managers;

public class TileSetManager : IManager<TileSet>
{
    private const string ASSETS_LOCATION = "avares://ColorTiles/Assets";
    private static readonly string AssetLocationRelativeDefaultTilesetPath = "Tilesets/Color-Tiles.png";

    public List<TileSet> TileSets { get; private set; }

    public TileSetManager()
    {
        TileSets = new List<TileSet>();
    }

    public override void Add(TileSet tileSet)
    {
        tileSet.ID = TileSets.Count;
        TileSets.Add(tileSet);
    }

    public override void Remove(TileSet tileSet)
    {
        TileSets.Remove(tileSet);

        RewriteIDs();
    }

    public override void Remove(int index)
    {
        TileSets.RemoveAt(index);

        RewriteIDs();
    }

    public void RewriteIDs()
    {
        for (int i = 0; i < TileSets.Count; i++)
        {
            TileSets[i].ID = i;
        }
    }

    public override TileSet Get(int id)
    {
        return TileSets[id];
    }

    #region Load 

    public override DefaultTileSet LoadDefault()
    {
        if (!TryLoadImageAsset(AssetLocationRelativeDefaultTilesetPath, out IImage? image))
        {
            throw new Exception($"Failed to default tileset from {ASSETS_LOCATION}/{AssetLocationRelativeDefaultTilesetPath}.");
        }

        return new DefaultTileSet(0, image!);
    }

    public static T LoadTileset<T>(string path) where T : GameTileSet, new()
    {
        // later on we might add support for custom tilesets
        if (!TryLoadImageAsset(path, out IImage? image))
        {
            throw new Exception("Failed to load image asset.");
        }

        return new T();
    }

    private static bool TryLoadImageAsset(string path, out IImage? output)
    {
        output = null;

        try
        {
            output = new Bitmap(AssetLoader.Open(new Uri($"{ASSETS_LOCATION}/{path}")));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #endregion

    #region Disposal

    public override void Dispose()
    {
        foreach (var tileSet in TileSets)
        {
            tileSet.Dispose();
        }
    }

    #endregion
}
