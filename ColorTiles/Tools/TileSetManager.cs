using System.Collections.Generic;
using ColorTiles.Entities.Tools;

namespace ColorTiles.Tools
{
    public class TileSetManager
    {
        public List<TileSet> TileSets { get; private set; }

        public TileSetManager()
        {
            TileSets = new List<TileSet>();
        }

        public void AddTileSet(TileSet tileSet)
        {
            tileSet.ID = TileSets.Count;
            TileSets.Add(tileSet);
        }

        public void RemoveTileSet(TileSet tileSet)
        {
            TileSets.Remove(tileSet);

            RewriteIDs();
        }

        public void RemoveTileSet(int index)
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

        public TileSet GetTileSet(int id)
        {
            return TileSets[id];
        }
    }
}