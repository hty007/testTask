using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowFree
{
    public class GameData
    {
        private List<Level> levels = new List<Level>();

        public void AddLevel(Level level) => levels.Add(level);

        public IReadOnlyCollection<string> GetLevelNames() => levels.Select(l => l.Name).ToList();
    }
}