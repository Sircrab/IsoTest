using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ChunkManager
{
    private static string baseFolder = Application.dataPath;

    private class Pair
    {
        public int x, y;

        public Pair(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class InvalidMapSizeException : Exception
    {
        public InvalidMapSizeException(string message) : base(message) { }
    }

    public static void Chunkify(Tile[][][] map)
    {
        //TODO: Get rid of this hardcoded path pls
        string finalPath = baseFolder + @"\Chunks";
        Directory.CreateDirectory(Path.GetDirectoryName(finalPath));
        IFormatter formatter = new BinaryFormatter();
        ValidateSize(ref map);
        int mapHeight = map[0].Length;
        int mapWidth = map[0][0].Length;
        Dictionary<Pair, Chunk> chunks = new Dictionary<Pair, Chunk>(); 

        for (int i = map.Length - 1 ; i >= 0; i-- )
        {
            for (int j = 0; j < mapHeight; j++ )
            {
                for (int k = 0; k < mapWidth; k++ )
                {
                    if (!map[i][j][k].IsEmpty())
                    {
                        Chunk chunk;
                        Pair accessPair = new Pair(j / Chunk.c_chunkHeight, k / Chunk.c_chunkWidth);
                        if (!chunks.TryGetValue(accessPair,out chunk))
                        {
                            chunks[accessPair] = new Chunk(i+1);
                            chunks[accessPair].chunkRow = accessPair.x;
                            chunks[accessPair].chunkColumn = accessPair.y;                      
                        }
                        if (chunk != null )
                        {
                            chunk.m_tiles[i][j % Chunk.c_chunkHeight][k % Chunk.c_chunkWidth] = map[i][j][k];
                        }
                    }
                }
            }
        }

        foreach (Chunk chunk in chunks.Values )
        {
            //TODO: hardcoded path
            Stream stream = new FileStream(finalPath + @"\" + chunk.chunkRow.ToString() + "_" + chunk.chunkColumn.ToString() + ".dat", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, chunk);
            stream.Close();
        }
    }

    private static Chunk GetChunk(int row, int column)
    {
        string finalPath = baseFolder + @"\Chunks";
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(finalPath + @"\" + row.ToString() + "_" + column.ToString() + ".dat", FileMode.Open);
        Chunk chunk = null;
        try
        {
            chunk = (Chunk)formatter.Deserialize(stream);
        }
        catch (SerializationException e)
        {
            Debug.LogError("Critical serialization error");
        }
        finally
        {
            stream.Close();
        }
        return chunk;
    }

    private static void ValidateSize(ref Tile[][][] map)
    {
        for (int i = 0; i < map.Length; i++ )
        {
            if(map[i].Length % Chunk.c_chunkHeight != 0)
            {
                throw new InvalidMapSizeException(String.Format("Map has invalid height, must be divisible by %d", Chunk.c_chunkHeight));
            }
            if(map[i][0].Length % Chunk.c_chunkWidth != 0 )
            {
                throw new InvalidMapSizeException(String.Format("MAp has invalid width, must be divisible by %d", Chunk.c_chunkWidth));
            }
        }
    }
}
