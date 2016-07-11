using UnityEngine;


public interface IMeshCreator
{
    void CreateMeshFilterWithProperties(
        Vector3[] vertices, Vector2[] uvCoords, int[] triangles, Vector3[] normals);
}