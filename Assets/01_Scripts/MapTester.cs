using UnityEngine;

public class MapTester : MonoBehaviour {
	// Use this for initialization
	void Start ()
    {
        Tile[][] map = new Tile[3][];
        for(int i = 0; i < 3; i++)
        {
            map[i] = new Tile[3];
            for(int j = 0; j < 3; j++)
            {
                map[i][j] = new Tile();
                if((j + i) % 2 == 0)
                {
                    map[i][j].id = 1;
                }
            }
        }
        Map.instance.MakeMap(map);
	}
}
