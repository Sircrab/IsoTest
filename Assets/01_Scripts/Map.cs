using UnityEngine;
using System.Linq;
using System;

public class Map : MonoSingleton<Map>
{
    public const float tileSize = 5f;

    [SerializeField]
    private MeshFilter m_meshFilter;
    private Tile[][] m_tiles;

    public Tile[][] Tiles
    {
        set { m_tiles = value; }
    }

    public void MakeMap(Tile[][] tiles)
    {
        if (tiles == null 
            || tiles.Any(x => x == null) 
            || tiles.Any(x => x.Length <= 0) 
            || Array.Exists(tiles, x => x.Length != tiles[0].Length))
        {
            throw new InvalidTileSizeException("Tiles array must be non-empty rectangular shaped");
        }
        m_tiles = tiles;
        int tilesHeight = m_tiles.Length;
        int tilesWidth = m_tiles[0].Length;
        int verticesWidth = tilesWidth + 1;
        int verticesHeight = tilesHeight + 1;
        int numVertices = verticesWidth * verticesHeight;
        Vector3[] mapVertices = new Vector3[numVertices * 4];
        //Vertices
        for (int i = 0; i < numVertices; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                mapVertices[i * 4 + j] = new Vector3(i % verticesWidth * tileSize, 0, i / verticesWidth * tileSize);
            }

        }
        int[] triangles = CreateTriangles(verticesWidth, verticesHeight);
        Vector3[] normals = CreateNormals(numVertices);
        Vector2[] uvCoords = CreateUVs(tilesHeight, tilesWidth, verticesWidth, numVertices);

        m_meshFilter.mesh.vertices = mapVertices;
        m_meshFilter.mesh.uv = uvCoords;
        m_meshFilter.mesh.triangles = triangles;
        m_meshFilter.mesh.normals = normals;
    }

    private Vector2[] CreateUVs(int tilesHeight, int tilesWidth, int verticesWidth, int numVertices)
    {
        Vector2[] uvCoords = new Vector2[numVertices * 4];
        for (int i = 0; i < tilesHeight; i++)
        {
            for (int j = 0; j < tilesWidth; j++)
            {
                Sprite tileSprite = SpriteHolder.instance.GetSpriteByID(m_tiles[i][j].id);
                //Top left corner south
                uvCoords[(i * verticesWidth + j) * 4 + 0 + 2] = tileSprite.uv[1];
                //Top right corner west
                uvCoords[(i * verticesWidth + j + 1) * 4 + 3] = tileSprite.uv[2];
                //Bottom left corner north
                uvCoords[((i + 1) * verticesWidth + j) * 4 + 0] = tileSprite.uv[3];
                //Bottom right corner east
                uvCoords[((i + 1) * verticesWidth + j + 1) * 4 + 1] = tileSprite.uv[0];
            }
        }
        return uvCoords;
    }

    private static Vector3[] CreateNormals(int numVertices)
    {
        Vector3[] normals = new Vector3[numVertices * 4];
        for (int i = 0; i < numVertices * 4; i++)
        {
            normals[i] = Vector3.up;
        }
        return normals;
    }

    private static int[] CreateTriangles(int verticesWidth, int verticesHeight)
    {
        int triIndex = 0;
        int[] triangles = new int[2 * 3 * verticesWidth * verticesHeight];
        for (int i = 0; i < verticesHeight - 1; i++)
        {
            for (int j = 0; j < verticesWidth - 1; j++)
            {
                //Sup tri
                triangles[triIndex++] = ((i + 1) * verticesWidth + j) * 4 + 0;                
                triangles[triIndex++] = (i * verticesWidth + j + 1) * 4 + 3;
                triangles[triIndex++] = (i * verticesWidth + j) * 4 + 0 + 2;
                //Lower tri
                triangles[triIndex++] = ((i + 1) * verticesWidth + j) * 4 + 0;             
                triangles[triIndex++] = ((i + 1) * verticesWidth + j + 1) * 4 + 1;
                triangles[triIndex++] = (i * verticesWidth + j + 1) * 4 + 3;
            }
        }
        return triangles;
    }

    public class InvalidTileSizeException : Exception
    {
        public InvalidTileSizeException(string message): base(message){ }      
    }
}


