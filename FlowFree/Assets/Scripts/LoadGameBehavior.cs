using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowFree
{
    public class LoadGameBehavior : MonoBehaviour
    {
        [SerializeField]
        private GameObject pleaseHold = null;
        [SerializeField]
        private Dropdown levelSelector = null;

        [Header("Next Level")]
        [SerializeField]
        private GameObject nextLevelPanel = null;
        [SerializeField]
        private Button nextLevelButton = null;
        [SerializeField]
        private Button repeatLevel = null;
        [SerializeField]
        private Button exit = null;

        private IGameController controller;
        private ISettings settings;
        private int currentIndexLevel;

        public void SetLavel(int index) => controller.SetLavel(index);

        private IEnumerator LoadLevels()
        {
            Task load = controller.FindLevels();
            yield return new WaitForEndOfFrame();
            pleaseHold.SetActive(true);
            while (!load.IsCompleted)
            {
                yield return new WaitForEndOfFrame();
            }
            currentIndexLevel = 0;
            controller.SetLavel(currentIndexLevel);
            pleaseHold.SetActive(false);
        }

        private void OnLevelLoaded()
        {
            levelSelector.ClearOptions();
            levelSelector.AddOptions(controller.LevelNames.ToList());
        }

        [Inject]
        private void Init(IGameController controller, ISettings settings)
        {
            this.controller = controller;
            this.settings = settings;
        }

        private void Start()
        {
            if (controller == null)
            {
                Debug.Log("controller is null");
                return;
            }

            levelSelector.gameObject.SetActive(settings.SelectLavel);
            if (settings.SelectLavel)
            {
                controller.LevelsLoad += OnLevelLoaded;
            }

            controller.NextLevelLoad += OnNextLevelLoad;
            levelSelector.onValueChanged.AddListener(SetLavel);
            StartCoroutine(LoadLevels());

            pleaseHold.SetActive(false);
            nextLevelPanel.SetActive(false);

            nextLevelButton.onClick.AddListener(NextLevelClick);
            repeatLevel.onClick.AddListener(RepeatLevelClick);
            exit.onClick.AddListener(ExitClick);

        }

        private void ExitClick()
        {
            Application.Quit();
        }

        private void OnNextLevelLoad()
        {
            nextLevelPanel.SetActive(true);
        }

        private void RepeatLevelClick()
        {
            controller.SetLavel(currentIndexLevel);
            nextLevelPanel.SetActive(false);
        }

        private void NextLevelClick()
        {
            controller.SetLavel(++currentIndexLevel);
            nextLevelPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            controller.LevelsLoad -= OnLevelLoaded;
            levelSelector.onValueChanged.RemoveListener(SetLavel);
            
        }
    }
}