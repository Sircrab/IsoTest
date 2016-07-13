using NUnit.Framework;
using NSubstitute;
using UnityEngine;

[TestFixture()]
public class MapControllerTest
{
    private MapController mapController;
    private IMeshCreator meshCreator;
    private ISpriteDictionary spriteDictionary;

    [SetUp()]
    public void Before()
    {
        mapController = GetMapControllerMock();
        spriteDictionary = GetSpriteDictionaryMock();
        meshCreator = GetMeshCreatorMock();

        mapController.SetSpriteDictionary(spriteDictionary);
        mapController.SetMeshCreator(meshCreator);
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

    [Test()]
    public void MakeMap_CreatesMesh_OnValidDimensions()
    {
        int tileSize = 2;
        Tile[][] tiles = new Tile[tileSize][];
        for (int i = 0; i < tileSize; i++)
        {
            tiles[i] = new Tile[tileSize];
        }
        for (int i = 0; i < tileSize; i++)
        {
            for (int j = 0; j < tileSize; j++)
            {
                tiles[i][j] = new Tile();
                tiles[i][j].id = 0;
            }
        }

        mapController.MakeMap(tiles, 0);

        meshCreator.Received().CreateMeshFilterWithProperties(
            Arg.Any<Vector3[]>(), Arg.Any<Vector2[]>(), Arg.Any<int[]>(), Arg.Any<Vector3[]>(), Arg.Any<int>());
    }

    private IMeshCreator GetMeshCreatorMock()
    {
        return Substitute.For<IMeshCreator>();
    }

    private MapController GetMapControllerMock()
    {
        return Substitute.For<MapController>();
    }

    private ISpriteDictionary GetSpriteDictionaryMock()
    {
        ISpriteDictionary spriteDictionary = Substitute.For<ISpriteDictionary>();
        spriteDictionary.GetSpriteByID(Arg.Any<int>()).Returns(GetDummySprite());
        return spriteDictionary;
    }

    private Sprite GetDummySprite()
    {
        Sprite sprite = 
            Sprite.Create(Texture2D.blackTexture, Rect.MinMaxRect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        return sprite;
    }
}