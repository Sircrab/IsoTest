using NUnit.Framework;
using System.Linq;

[TestFixture()]
public class MapTest
{
    private Map instance;
    [SetUp()]
    public void Init()
    {
        instance = Map.instance;
    }

    [Test()]
    public void MapMake_throwsException_onEmptyArray()
    {
        Assert.Throws<Map.InvalidTileSizeException>(() => instance.MakeMap(new Tile[3][][]));
    }

    [Test()]
    public void MapMake_throwsException_onJaggedArray()
    {
        Tile[][][] tiles = new Tile[1][][];
        tiles[0][0] = new Tile[3];
        tiles[0][1] = new Tile[2];
        tiles[0][2] = new Tile[4];

        Assert.Throws<Map.InvalidTileSizeException>(() => instance.MakeMap(tiles));
    }
}