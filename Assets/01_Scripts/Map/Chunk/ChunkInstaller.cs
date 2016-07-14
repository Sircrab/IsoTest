using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Zenject;

public class ChunkInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.Bind<ChunkManager>().AsSingle();
        Container.Bind<IFormatter>().To<BinaryFormatter>().AsSingle();
    }
}