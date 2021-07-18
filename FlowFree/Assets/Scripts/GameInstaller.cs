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
            Container.Bind<GameData>().AsSingle();

            Container.Bind<Settings>().FromMethod(GetSettings);

        }

        private Settings GetSettings()
        {
            return GameObject.FindObjectOfType<Settings>();
        }
    }
}