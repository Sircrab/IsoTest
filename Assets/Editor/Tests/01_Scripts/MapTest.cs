using NUnit.Framework;
using NSubstitute;

[TestFixture()]
public class MapTest
{
    private MapController mapController;

    [SetUp()]
    public void Before()
    {
        mapController = Substitute.For<MapController>();
    }

    [Test()]
    public void MakeMap_ThrowsException_OnEmptyArray()
    {
        Assert.Throws<MapController.InvalidTileSizeException>(
            () => mapController.MakeMap(new Tile[5][], 0));
    }

    [Test()]
    public void MakeMap_ThrowsException_OnJaggedArray()
    {
        Tile[][] tiles = new Tile[3][];
        tiles[0] = new Tile[2];
        tiles[1] = new Tile[3];
        tiles[2] = new Tile[1];

        Assert.Throws<MapController.InvalidTileSizeException>(
            () => mapController.MakeMap(tiles, 0));
    }
}