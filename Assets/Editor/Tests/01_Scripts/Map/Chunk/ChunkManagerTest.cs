using NUnit.Framework;
using Zenject;

[TestFixture]
public class ChunkManagerTest
{
    DiContainer container;
    ChunkManager manager;

    [SetUp]
    public void Before()
    {
        container = new DiContainer();
        container.Install<ChunkInstaller>();
        manager = container.Resolve<ChunkManager>();
    }

    [Test]
    public void Nothing()
    {
        // manager is usable here now
    }
}

