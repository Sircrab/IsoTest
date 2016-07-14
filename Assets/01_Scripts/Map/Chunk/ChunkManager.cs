using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ChunkManager
{
    public class InvalidMapSizeException : Exception
    {
        public InvalidMapSizeException(string message) : base(message) { }
    }

    private class Pair
    {
        public int x, y;

        public Pair(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private static string baseFolder = Application.dataPath;
    private static IFormatter formatter = new BinaryFormatter();

    public static void Chunkify(Tile[][][] map)
    {
        //TODO: Get rid of this hardcoded path pls
        string finalPath = baseFolder + @"\Chunks";
        Directory.CreateDirectory(Path.GetDirectoryName(finalPath));
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
                        Pair accessPair = 
                            new Pair(j / Chunk.c_chunkHeight, k / Chunk.c_chunkWidth);
                        if (!chunks.TryGetValue(accessPair,out chunk))
                        {
                            chunks[accessPair] = new Chunk(i+1);
                            chunks[accessPair].row = accessPair.x;
                            chunks[accessPair].col = accessPair.y;                      
                        }
                        if (chunk != null)
                        {
                            chunk.m_tiles[i][j % Chunk.c_chunkHeight][k % Chunk.c_chunkWidth] = 
                                map[i][j][k];
                        }
                    }
                }
            }
        }
    }

    private static void SaveChunks(ICollection<Chunk> chunks, String saveDirectory)
    {
        foreach (Chunk chunk in chunks)
        {
            SaveChunk(chunk, saveDirectory);
        }
    }

    private static void SaveChunk(Chunk chunk, String saveDirectory)
    {
        Stream stream = new FileStream(
                saveDirectory + @"\" + chunk.row.ToString() + "_" + chunk.col.ToString() + ".dat",
                FileMode.Create,
                FileAccess.Write,
                FileShare.None);
        formatter.Serialize(stream, chunk);
        stream.Close();
    }

    private static Chunk LoadChunk(int row, int column)
    {
        string finalPath = baseFolder + @"\Chunks";
        Chunk chunk = null;
        Stream stream = null;
        try
        {
            stream = new FileStream(
            finalPath + @"\" + row.ToString() + "_" + column.ToString() + ".dat",
            FileMode.Open);
            chunk = (Chunk)formatter.Deserialize(stream);
        }
        catch (SerializationException e)
        {
            Debug.LogError(string.Format("Critical serialization error: %s", e.Message));
            throw e;
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
        return chunk;
    }

    private static void ValidateSize(ref Tile[][][] map)
    {
        for (int i = 0; i < map.Length; i++ )
        {
            if (map[i].Length % Chunk.c_chunkHeight != 0)
            {
                throw new InvalidMapSizeException(String.Format(
                    "Map has invalid height, must be divisible by %d", Chunk.c_chunkHeight));
            }
            if (map[i][0].Length % Chunk.c_chunkWidth != 0)
            {
                throw new InvalidMapSizeException(String.Format(
                    "Map has invalid width, must be divisible by %d", Chunk.c_chunkWidth));
            }
        }
    }
}
