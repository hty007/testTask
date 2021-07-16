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
        private static readonly string PATH_LEVELS = Path.Combine(Application.streamingAssetsPath + "Levels");

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
            var log = Renat.Auto();
            await Task.Run(() =>
            {
                log.Property(nameof(PATH_LEVELS), PATH_LEVELS);
                DirectoryInfo info = new DirectoryInfo(PATH_LEVELS);
                log.Property(nameof(info.FullName), info.FullName);
                var levels = info.GetFiles();

                foreach (var item in levels)
                {
                    log.Property(nameof(item.Name), item.Name);
                    var inputs = File.ReadAllLines(item.FullName);
                    Level level = DataHelper.ParseLevel(inputs);
                    if (level == null)
                    {
                        Debug.LogWarning($"Внимание уровень '{item.Name}' не квадратный!");
                        continue;
                    }
                    level.Name = Path.GetFileNameWithoutExtension(item.Name);
                    log.Property("level", level.Name);
                    data.AddLevel(level);
                }
            });
            log.AddText("всё");
            LevelChenge?.Invoke();
        }
    }
}