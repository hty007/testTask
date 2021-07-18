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
        private IGameController game;
        private IPlaceController place;
        private GridLayoutGroup gridLayout;
        private List<CellPlace> cells;

        private void Fill()
        {
            // удаление старых ячеек
            cells.ForEach(x => Destroy(x.gameObject));
            cells.Clear();

            // установка размера ячеек
            Level level = game.Current;
            int count = level.Count;
            float height = root.rect.height;
            root.sizeDelta = new Vector2(height, 0);

            RectOffset padding = gridLayout.padding;
            Vector2 spacing = gridLayout.spacing;
            float cellHeight = ((height - padding.top - padding.bottom) / count) - spacing.y;
            gridLayout.cellSize = new Vector2(cellHeight, cellHeight);

            // Todo костыль убрать обязательно
            CellPlace.controller.SetCount(count);

            // Создание новых ячеек
            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < count; i++)
                {
                    // TODO: in factory
                    var cell = Instantiate<CellPlace>(cellPrefab, root);
                    cell.SetPosition(i, j);

                    if (level[i, j] != 0)
                        cell.SetValue(level[i, j]);
                    
                    cells.Add(cell);
                }
            }
        }

        [Inject]
        private void Init(IGameController game, /*IPlaceController place,*/ Settings settings)
        {
            this.game = game;
            // this.place = place;
            // TODO move to factory
            CellPlace.settings = settings;
            //CellPlace.controller = place;
        }

        private void Start()
        {
            cells = new List<CellPlace>();
            game.CurrentChange += Fill;

            var height = root.rect.size.y;
            root.sizeDelta = new Vector2(height, 0);

            CellPlace.controller = new PlaceController(game);

            gridLayout = GetComponent<GridLayoutGroup>();
        }

    }
}
