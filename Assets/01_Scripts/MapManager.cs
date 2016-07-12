using UnityEngine;

public class MapManager : MonoSingleton<MapManager>
{
    //Order top left to bottom right
    [SerializeField]
    private Map[] m_maps;
    
    public void Reload(ChunkSet chunk)
    {
        for (int i = 0; i < m_maps.Length; i++ )
        {
            m_maps[i].MakeMap(chunk.m_tiles[i]);
            m_maps[i].transform.position = 
                new Vector3(
                    ( i / 3 ) * m_maps[i].controller.WorldWidth, 
                    0, 
                    ( i % 3 ) * m_maps[i].controller.WorldHeight);
        }
    }
}
