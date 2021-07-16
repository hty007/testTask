using Zenject;

namespace FlowFree
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameController>().To<GameController>().AsSingle();
            Container.Bind<GameData>().AsSingle();

        }
    }
}