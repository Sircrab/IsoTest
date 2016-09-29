using NUnit.Framework;

[TestFixture()]
public class EmptyTileTest
{
    [Test()]
    public void IsEmpty_ReturnsTrue()
    {
        Assert.That(new EmptyTile().IsEmpty());
    }
}