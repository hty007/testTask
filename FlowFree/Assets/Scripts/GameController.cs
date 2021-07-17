using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace FlowFree
{
    public class GameController : IGameController
    {
        #region fields
        private static readonly string PATH_LEVELS = Path.Combine(Application.streamingAssetsPath, "Levels");
        private List<string> levelNames = new List<string>();
        private GameData data;
        private Level current;
        #endregion
        #region ctor
        public GameController(GameData data) => this.data = data;
        #endregion
        #region property and event
        public IReadOnlyCollection<string> LevelNames => data.GetLevelNames();
        public Level Current => current;

        public event Action LevelsLoad;
        public event Action CurrentChange;
        #endregion
        #region public methods
        public async Task FindLevels()
        {
            await Task.Run(async () =>
            {
                DirectoryInfo info = new DirectoryInfo(PATH_LEVELS);
                var levels = info.GetFiles();

                foreach (var item in levels)
                {
                    if (item.Extension == ".meta")
                        continue;
                    var inputs = File.ReadAllLines(item.FullName);
                    Level level = DataHelper.ParseLevel(inputs);
                    if (level == null)
                    {
                        var txt = string.Join("\n", inputs);
                        Debug.LogWarning($"Внимание уровень '{item.Name}' не квадратный!\n{txt}");
                        continue;
                    }
                    level.Name = Path.GetFileNameWithoutExtension(item.Name);
                    data.AddLevel(level);
                    await Task.Delay(500);
                }
            });
            LevelsLoad?.Invoke();
        }

        public void SetLavel(int index)
        {
            current = data.GetLavel(index);
            CurrentChange?.Invoke();
        }
        #endregion
        #region private methods

        #endregion
    }
}