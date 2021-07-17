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
        private static readonly string PATH_LEVELS = Path.Combine(Application.streamingAssetsPath , "Levels");

        private List<string> levelNames = new List<string>();

        private GameData data;

        public GameController(GameData data)
        {
            this.data = data;
        }

        public IReadOnlyCollection<string> LevelNames => data.GetLevelNames();

        public event Action LevelChenge;

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
            LevelChenge?.Invoke();
        }
    }
}