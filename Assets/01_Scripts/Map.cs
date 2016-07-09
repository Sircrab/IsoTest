using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class Map : MonoSingleton<Map>
{
    [SerializeField]
    private Material m_material;

    private List<MeshFilter> m_subMeshes;

    public const float tileSize = 1f;
    public const float tileHeight = 0.5f;

    private Tile[][] m_tiles;
    public Tile[][] Tiles
    {
        set { m_tiles = value; }
    }

    public void MakeMap(Tile[][][] tiles)
    {
        m_subMeshes = new List<MeshFilter>();
        //Process all of the meshes here
        for (int i = 0; i < tiles.Length; i++)
        {
            //Add dimensional check
            MakeMap(tiles[i], i);
        }
        /*
        CombineInstance[] combine = new CombineInstance[m_subMeshes.Count];
        for (int i = 0; i < combine.Length; i++ )
        {
            combine[i].mesh = m_subMeshes[i].mesh;
            combine[i].transform = m_subMeshes[i].transform.localToWorldMatrix;
            m_subMeshes[i].gameObject.SetActive(false);
        }
        m_meshFilter.mesh.CombineMeshes(combine);
        */

    }

    public void MakeMap(Tile[][] tiles,int y)
    {
        if (tiles == null 
            || tiles.Any(x => x == null) 
            || tiles.Any(x => x.Length <= 0) 
            || Array.Exists<Tile[]>(tiles, x => x.Length != tiles[0].Length))
        {
            throw new InvalidTileSizeException("Tiles array must be non-empty rectangular shaped");
        }
        //New meshes need to be inside a gameObject
        GameObject meshHolder = new GameObject();
        MeshFilter subMesh = meshHolder.AddComponent<MeshFilter>();
        MeshRenderer renderer = meshHolder.AddComponent<MeshRenderer>();
        renderer.material = m_material;
        meshHolder.transform.SetParent(transform);

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
                mapVertices[i * 4 + j] = new Vector3(i % verticesWidth * tileSize, y * tileHeight, i / verticesWidth * tileSize);
            }

        }
        int[] triangles = CreateTriangles(verticesWidth, verticesHeight);
        Vector3[] normals = CreateNormals(numVertices);
        Vector2[] uvCoords = CreateUVs(tilesHeight, tilesWidth, verticesWidth, numVertices);

        subMesh.mesh.vertices = mapVertices;
        subMesh.mesh.uv = uvCoords;
        subMesh.mesh.triangles = triangles;
        subMesh.mesh.normals = normals;
        m_subMeshes.Add(subMesh);
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


