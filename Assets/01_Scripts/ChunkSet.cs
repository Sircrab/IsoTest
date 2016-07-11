using System.Linq;
using System.Collections.Generic;
using System;

public class ChunkSet
{
    public List<Tile[][][]> m_tiles;
    public const int TOTAL_TILESETS = 9;

    private ChunkSet(List<Tile[][][]> tiles)
    {
        m_tiles = tiles;
    }
    
    public static ChunkSetBuilder NewBuilder()
    {
        return new ChunkSetBuilder();
    }

    public class ChunkSetBuilder
    {
        private List<Tile[][][]> tiles;

        public ChunkSetBuilder()
        {
            tiles = new List<Tile[][][]>();
        }

        public ChunkSetBuilder Add(Tile[][][] tileSet)
        {
            tiles.Add(tileSet);
            return this;
        }

        public ChunkSetBuilder Remove(Tile[][][] tileSet)
        {
            tiles.Remove(tileSet);
            return this;
        }

        public ChunkSet Build()
        {
            if (tiles.Count == TOTAL_TILESETS)
            {
                if (tiles.Any(x => x== null) )
                {
                    throw new NullReferenceException("Found null tileset while building.");
                }
                tiles.Sort(delegate (Tile[][][] x, Tile[][][] y)
                {
                    return x[0][0][0].CompareTo(y[0][0][0]);
                });
                return new ChunkSet(tiles);
            }
            throw new InvalidTilesetCountException(
                string.Format(
                    "Expected %d tilesets, found %d while building.", TOTAL_TILESETS, tiles.Count));
        }
        
        public class InvalidTilesetCountException: Exception
        {
            public InvalidTilesetCountException(string message) : base(message) { }
        }
    }
}
