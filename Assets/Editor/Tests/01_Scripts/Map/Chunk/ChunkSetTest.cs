using NUnit.Framework;
using System;

[TestFixture()]
public class ChunkSetTest
{
    [Test()]
    public void Build_ThrowsException_IfNotEnoughTilesets()
    {
        Assert.Throws<ChunkSet.ChunkSetBuilder.InvalidTilesetCountException>(
            () => ChunkSet.NewBuilder()
                          .Add(new Chunk(new Tile[1][][]))
                          .Add(new Chunk(new Tile[1][][]))
                          .Build(), 
            "found 2");
    }

    [Test()]
    public void Build_ThrowsException_IfTooManyTilesets()
    {
        int numTilesets = 15;
        ChunkSet.ChunkSetBuilder builder = ChunkSet.NewBuilder();
        for (int i = 0; i < numTilesets; i++)
        {
            builder.Add(new Chunk(new Tile[1][][]));
        }

        Assert.Throws<ChunkSet.ChunkSetBuilder.InvalidTilesetCountException>(
            () => builder.Build(),
            "found 15.");
    }

    [Test()]
    public void Build_ThrowsException_IfNullTileset()
    {
        int numTilesets = 9;
        ChunkSet.ChunkSetBuilder builder = ChunkSet.NewBuilder();
        for (int i = 0; i < numTilesets; i++)
        {
            builder.Add(null);
        }

        Assert.Throws<NullReferenceException>(
            () => builder.Build(),
            "null");
    }
}
