using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Zenject;

public class ChunkInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.Bind<IFormatter>().To<BinaryFormatter>().FromInstance(new BinaryFormatter()).AsSingle();
        Container.Bind<ChunkManager>().AsSingle();
    }
}