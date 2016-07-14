using UnityEngine;
using System.Collections;

[System.Serializable]
public class Chunk
{
    public const int c_chunkWidth = 16;
    public const int c_chunkHeight = 16;

    public int chunkRow = 0, chunkColumn = 0;

    public Tile[][][] m_tiles;

    public Chunk(Tile[][][] tiles)
    {
        m_tiles = tiles;
    }

    public Chunk(int height)
    {
        m_tiles = new Tile[height][][];
        for (int i = 0; i < height; i++ )
        {
            m_tiles[i] = new Tile[c_chunkHeight][];
            for (int j = 0; j < c_chunkHeight; j++ )
            {
                m_tiles[i][j] = new Tile[c_chunkWidth];
                for (int k = 0; k < c_chunkWidth; k++ )
                {
                    m_tiles[i][j][k] = new EmptyTile();
                }
            }
        }
    }
}

	
