using UnityEngine;

public class MapTester : MonoBehaviour {
	// Use this for initialization
	void Start ()
    {
        Tile[][][] map = new Tile[32][][];
        for(int i = 0; i < 32; i++)
        {
            map[i] = new Tile[32][];
            for(int j = 0; j < 32; j++)
            {
                map[i][j] = new Tile[32];
                for(int k = 0; k < 32; k++ )
                {
                    map[i][j][k] = new Tile();
                    if((j+k)%2 == 0 )
                    {
                        map[i][j][k].id = 1;
                    }
                    else
                    {
                        map[i][j][k].id = 2;
                    }
                }
            }
        }
        Map.instance.MakeMap(map);
	}
}
