using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowFree
{
    public interface IGameController
    {
        IReadOnlyCollection<string> LevelNames { get; }

        event Action LevelsLoad;

        Task FindLevels();
        void SetLavel(int v);
    }
}