using UnityEngine;
using Zenject;

namespace FlowFree
{
    public class GameBehavior : MonoBehaviour
    {
        private IGameController controller;

        private void Start()
        {
            if (controller == null)
            {
                Debug.Log("controller null");
                return;
            }

            controller.FindLevels();
        }

        [Inject]
        private void Init(IGameController controller)
        {
            this.controller = controller;
        }
    }
}