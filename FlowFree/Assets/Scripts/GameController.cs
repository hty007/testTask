using System.IO;
using UnityEngine;

namespace FlowFree
{
    public class GameController : IGameController
    {

        private static readonly string PATH_LEVELS = Path.Combine(Application.streamingAssetsPath + "Levels");

        private GameData data;

        public GameController(GameData data)
        {
            this.data = data;
        }

        public void FindLevels()
        {
            DirectoryInfo info = new DirectoryInfo(PATH_LEVELS);
             info.GetFiles().Select()
        }
    }

    
}