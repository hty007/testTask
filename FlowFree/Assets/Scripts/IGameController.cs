using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowFree
{
    public interface IGameController
    {
        IReadOnlyCollection<string> LevelNames { get; }
        Level Current { get; }

        event Action LevelsLoad;
        event Action CurrentChange;
        event Action NextLevelLoad;

        Task FindLevels();
        void SetLavel(int v);
        void CurrentLevelCompalete();
    }
}