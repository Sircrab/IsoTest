using NUnit.Framework;

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
        Assert.Throws<Map.InvalidTileSizeException>(() => instance.MakeMap(new Tile[3][]));
    }

    [Test()]
    public void MapMake_throwsException_onJaggedArray()
    {
        Tile[][] tiles = new Tile[3][];
        tiles[0] = new Tile[3];
        tiles[1] = new Tile[2];
        tiles[2] = new Tile[4];

        Assert.Throws<Map.InvalidTileSizeException>(() => instance.MakeMap(tiles));
    }
}