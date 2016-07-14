using NUnit.Framework;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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
        // manager can be used here
    }
}

