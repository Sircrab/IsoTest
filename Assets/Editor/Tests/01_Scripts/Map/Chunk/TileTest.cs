using NUnit.Framework;

[TestFixture()]
public class TileTest
{
    [Test()]
    public void CompareTo_PrioritizesHeight()
    {
        Tile t1 = new Tile(3, 1, 3);
        Tile t2 = new Tile(3, 2, 3);

        Assert.That(t1.CompareTo(t2) == -1);
    }

    [Test()]
    public void CompareTo_PrioritizesRow_OnEqualHeight()
    {
        Tile t1 = new Tile(3, 3, 1);
        Tile t2 = new Tile(3, 3, 2);

        Assert.That(t1.CompareTo(t2) == -1);
    }

    [Test()]
    public void CompareTo_PrioritizesColumn_OnEqualHeighAndRow()
    {
        Tile t1 = new Tile(1, 3, 3);
        Tile t2 = new Tile(2, 3, 3);

        Assert.That(t1.CompareTo(t2) == -1);
    }
}