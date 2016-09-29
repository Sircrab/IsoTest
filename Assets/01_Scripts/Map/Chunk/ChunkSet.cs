using System.Linq;
using System.Collections.Generic;
using System;

public class ChunkSet
{
    public List<Chunk> m_chunks;
    public const int TOTAL_TILESETS = 9;
    

    private ChunkSet(List<Chunk> chunks)
    {
        m_chunks = chunks;
    }
    
    public static ChunkSetBuilder NewBuilder()
    {
        return new ChunkSetBuilder();
    }

    public class ChunkSetBuilder
    {
        private List<Chunk> tiles;

        public ChunkSetBuilder()
        {
            tiles = new List<Chunk>();
        }

        public ChunkSetBuilder Add(Chunk tileSet)
        {
            tiles.Add(tileSet);
            return this;
        }

        public ChunkSetBuilder Remove(Chunk tileSet)
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
                tiles.Sort(delegate (Chunk x, Chunk y)
                {
                    return x.m_tiles[0][0][0].CompareTo(y.m_tiles[0][0][0]);
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
