using System;
using UnityEngine;
using Zenject;

namespace FlowFree
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameController>().To<GameController>().AsSingle();
            Container.Bind<IPlaceController>().To<PlaceController>().AsSingle();
            Container.Bind<GameData>().AsSingle();

            Container.Bind<ISettings>().FromMethod(GetSettings).AsSingle();

            // Todo Factory
            // Container.BindFactory<CellFactory>

        }

        private Settings GetSettings()
        {
            return GameObject.FindObjectOfType<Settings>();
        }
    }
}