using UnityEngine;
using System;

public class Map : MonoBehaviour, IMeshCreator, IChildrenManager
{
    public MapController controller;

    [SerializeField]
    private Material m_material;

    public void OnEnable()
    {
        controller.SetMeshCreator(this);
        controller.SetChildrenManager(this);
    }

    public void MakeMap(Tile[][][] tiles)
    {
        controller.MakeMap(tiles);
    }

    public void MakeMap(Tile[][] tiles, int y)
    {
        controller.MakeMap(tiles, y);
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

    #region IChildrenManager implementation
    public void DestroyChildrenGameObjects()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
    #endregion

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

    public class InvalidTileSizeException : Exception
    {
        public InvalidTileSizeException(string message): base(message){ }      
    }
}


