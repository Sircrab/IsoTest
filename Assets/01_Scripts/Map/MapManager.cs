using UnityEngine;

public class MapManager : MonoSingleton<MapManager>
{
    //Order top left to bottom right
    [SerializeField]
    private Map[] m_maps;

    [SerializeField]
    private int mapRows = 0;
    private int mapColumns = 0;
    
    public void Reload(ChunkSet chunk)
    {
        for (int i = 0; i < mapRows; i++ )
        {
            for (int j = 0; j < mapColumns; j++ )
            {
                int idx = i * mapRows + j;
                m_maps[idx].MakeMap(chunk.m_chunks[idx].m_tiles);
                m_maps[idx].transform.position =
                    new Vector3(
                        i * m_maps[idx].controller.WorldWidth,
                        0,
                        j * m_maps[idx].controller.WorldHeight);
            }
        }
    }
}
