using UnityEngine;
using System.Linq;
using System;

public class Map : MonoBehaviour, IMeshCreator
{
    public const float c_tileSize = 1f;
    public const float c_tileHeight = 0.5f;
    public MapController controller;

    [SerializeField]
    private Material m_material;
    private Tile[][] m_tiles;
    private float m_worldWidth = 0f;
    private float m_worldHeight = 0f;

    public float WorldWidth
    {
        get { return m_worldWidth; }
    }

    public float WorldHeight
    {
        get { return m_worldHeight; }
    }

    public Tile[][] Tiles
    {
        set { m_tiles = value; }
    }

    public void OnEnable()
    {
        controller.SetMeshCreator(this);
    }

    public void MakeMap(Tile[][][] tiles)
    {
        DestroyChildrenGameObjects();
        //Process all of the meshes here
        for (int i = 0; i < tiles.Length; i++)
        {
            //Add dimensional check
            MakeMap(tiles[i], i);
        }
    }

    public void MakeMap(Tile[][] tiles, int y)
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
        m_worldWidth = tilesWidth * c_tileSize;
        m_worldHeight = tilesHeight * c_tileSize;

        Vector3[] mapVertices = CreateVertices(y, verticesWidth, numVertices);
        int[] triangles = CreateTriangles(verticesWidth, verticesHeight);
        Vector3[] normals = CreateNormals(numVertices);
        Vector2[] uvCoords = CreateUVs(tilesHeight, tilesWidth, verticesWidth, numVertices);

        CreateMeshFilterWithProperties(
            mapVertices, 
            uvCoords, 
            triangles, 
            normals);
    }
     
    #region IMeshCreator implementation
    public void CreateMeshFilterWithProperties(
        Vector3[] vertices, Vector2[] uvCoords, int[] triangles, Vector3[] normals)
    {
        MeshFilter subMesh = CreateChildMeshHolder().GetComponent<MeshFilter>();
        subMesh.mesh.vertices = vertices;
        subMesh.mesh.uv = uvCoords;
        subMesh.mesh.triangles = triangles;
        subMesh.mesh.normals = normals;
    }
    #endregion

    private static Vector3[] CreateVertices(int y, int verticesWidth, int numVertices)
    {
        Vector3[] mapVertices = new Vector3[numVertices * 4];
        //Vertices
        for (int i = 0; i < numVertices; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                mapVertices[i * 4 + j] = new Vector3(i % verticesWidth * c_tileSize,
                                                     y * c_tileHeight,
                                                     i / verticesWidth * c_tileSize);
            }
        }

        return mapVertices;
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

    private GameObject CreateChildMeshHolder()
    {
        GameObject meshHolder = new GameObject();
        meshHolder.AddComponent<MeshFilter>();
        MeshRenderer renderer = meshHolder.AddComponent<MeshRenderer>();
        renderer.material = m_material;
        meshHolder.transform.SetParent(transform);
        meshHolder.transform.localPosition = new Vector3(0, 0, 0);
        return meshHolder;
    }

    private void DestroyChildrenGameObjects()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

    public class InvalidTileSizeException : Exception
    {
        public InvalidTileSizeException(string message): base(message){ }      
    }
}


