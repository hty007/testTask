using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FlowFree
{
    public class GamePlacePresenter : MonoBehaviour
    {
        [SerializeField]
        private RectTransform root = null;

        [SerializeField]
        private CellPlace cellPrefab = null;
        private IGameController controller;
        private GridLayoutGroup gridLayout;
        private List<CellPlace> cells;

        private void Fill()
        {
            // удаление старых ячеек
            cells.ForEach(x => Destroy(x.gameObject));
            cells.Clear();

            // установка размера ячеек
            Level level = controller.Current;
            int count = level.Count;
            float height = root.rect.height;
            //height = root.rect.size.y;            
            //root.sizeDelta = new Vector2(height, height);

            RectOffset padding = gridLayout.padding;
            Vector2 spacing = gridLayout.spacing;
            float cellHeight = ((height - padding.top - padding.bottom) / count) - spacing.y;
            gridLayout.cellSize = new Vector2(cellHeight, cellHeight);

            // Создание новый ячеек
            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < count; i++)
                {
                    // TODO: in factory
                    var cell = Instantiate<CellPlace>(cellPrefab, root);
                    cell.SetPosition(i, j);
                    if (level[i, j] != 0)
                        cell.SetValue(level[i, j]);
                    cell.SetPosition(i, j);
                    
                    cells.Add(cell);
                }
            }
        }

        [Inject]
        private void Init(IGameController controller, Settings settings)
        {
            this.controller = controller;
            CellPlace.settings = settings;
        }

        private void Start()
        {
            cells = new List<CellPlace>();
            controller.CurrentChange += Fill;

            var height = root.rect.size.y;
            root.sizeDelta = new Vector2(height, 0);
            

            gridLayout = GetComponent<GridLayoutGroup>();
        }

    }
}
