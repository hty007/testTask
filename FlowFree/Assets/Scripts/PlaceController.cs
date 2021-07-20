using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlowFree
{
    public class PlaceController : IPlaceController
    {
        private IGameController game;
        private ISettings settings;
        private Cell[,] data;
        private bool canMove;
        private Stack<Cell> line;
        private Dictionary<int, Stack<Cell>> lines;

        //private Dictionary<Vector2Int, PlaceChangeDelegate> listeners;

        public PlaceController(IGameController game, ISettings settings)
        {
            this.game = game;
            this.settings = settings;
            game.CurrentChange += OnCurrentChanged;
            lines = new Dictionary<int, Stack<Cell>>();
        }

        public int Count { get; private set; }

        public void AddListener(Vector2Int pos, PlaceChangeDelegate action)
        {
            var cell = data[pos.x, pos.y];

            cell.Invoke += action;

            RunAction(cell, TypeAction.Reset);
            if (cell.isRoot)
                RunAction(cell, TypeAction.Center);
            else
                RunAction(cell, TypeAction.CenterOff);

        }

        public void BeginLine(Vector2Int pos)
        {
            var log = Renat.Auto();
            log.AddText($"BeginLine {pos}");
            // Начало движения
            var cell = data[pos.x, pos.y];
            // Ячейка должнеа быть закрашена
            if (!cell.IsEmpty)
            { // Ячейка не пустая можно двигатся
                canMove = true;
                if (!lines.ContainsKey(cell.color))
                {
                    lines.Add(cell.color, new Stack<Cell>());
                }

                line = lines[cell.color];
                if (cell.isRoot)
                {
                    if (line.Count == 0)
                    {
                        line.Push(cell);
                    }
                    else
                    {
                        foreach (var item in line)
                        {
                            DoEmpty(item);
                        }
                    }
                }
                else
                {
                    Debug.Log("Продолжение движения?");
                }
            }
            //else
            //{
            //    Renat.Log("Ячейка пустая");
            //}
        }

        public void EndLine(Vector2Int pos)
        {
            var log = Renat.Auto(); 
            log.AddText($"EndLine {pos}");
            canMove = false;

            log.Property("count", line.Count);
        }

        public void Move(Vector2Int pos)
        {
            // Нажатый указатель попал в ячейку
            if (!canMove)
                return;

            var log = Renat.Auto();
            log.AddText($"Move {pos}");

            var cell = data[pos.x, pos.y];

            if (line.Count == 0 && cell.isRoot)
            {
                line.Push(cell);
                return;
            }
            if (line.Count == 0)
            {
                Debug.LogWarning("Движение слишком быстрое!");
                return;
            }

            Cell back = line.Peek();
            log.Property("back", back.position);
            if (Vector2Int.Distance(cell.position, back.position) != 1)
            {
                // Движение произошло по диоганали
                return;
            }
            else if (cell.IsEmpty)// Пустая ячейка
            {
                log.AddText("IsEmpty");
                AddCell(cell, back);
            }
            else if (!cell.IsEmpty)// Заполненая ячейка
            {
                if (back.color == cell.color) // Цвет совподает
                {
                    if (cell.isRoot)
                    {
                        //Цель достигнута
                        Connect(cell, back);
                    }
                    else
                    { //  cell уже есть в line обрезать line по cell
                        log.Add("Ячейка не пустая, цвет совподает");
                        var empty = line.Pop();
                        while (empty != cell)
                        {
                            DoEmpty(empty);                            
                            empty = line.Pop();
                            if (line.Count == 1)
                                break;
                        }

                        back = line.Peek();
                        DoEmpty(cell);

                        AddCell(cell, back);

                        log.Property("line.Count", line.Count);
                    }
                }
                else// Цвет не совподает
                {
                    if (cell.isRoot)
                    {
                        foreach (var item in line)
                        {
                            DoEmpty(item);
                        }
                    }
                    else
                    {
                        var line2 = lines[cell.color];
                        var empty = line2.Peek();
                        while (empty != cell)
                        {
                            if (line2.Count == 1)
                                continue;
                            empty = line2.Pop();
                            log.AddText($"Стираю {empty.position} [{empty.color}]");
                            DoEmpty(empty);
                        }

                        // Todo:  line2 [c]x[b]-[b]

                        cell.color = back.color;
                        Connect(cell, back);
                    }
                }
            }
            //else if (cell.isRoot)// Другая неизменная ячейка
            //{
            //    if (back.color == cell.color) // Цвет совподает
            //    {
            //        Connect(cell, back);
            //    }
            //    else// Цвет не совподает
            //    {
            //        // ломаем всю линию?
            //    }
            //}
        }

        private void AddCell(Cell cell, Cell back)
        {
            line.Push(cell);
            cell.color = back.color;
            Connect(cell, back);
        }

        private void DoEmpty(Cell empty)
        {
            if (!empty.isRoot)
                empty.color = 0;
            empty.Invoke(TypeAction.Reset, Color.white);
        }

        private void Connect(Cell cell, Cell back)
        {
            if (cell.Left == back)
            {
                RunAction(cell, TypeAction.Left);
                RunAction(back, TypeAction.Right);
            }
            else if (cell.Right == back)
            {
                RunAction(cell, TypeAction.Right);
                RunAction(back, TypeAction.Left);
            }
            else if (cell.Top == back)
            {
                RunAction(cell, TypeAction.Top);
                RunAction(back, TypeAction.Bottom);
            }
            else if (cell.Bottom == back)
            {
                RunAction(cell, TypeAction.Bottom);
                RunAction(back, TypeAction.Top);
            }


        }

        private bool TryPosition(int x, int y, int dx, int dy, out int n_x, out int n_y)
        {
            (n_x, n_y) = (x + dx, y + dy);
            return n_x >= 0 && n_x < Count && n_y >= 0 && n_y < Count;
        }

        private void RunAction(Cell cell, TypeAction type)
        {
            cell.Invoke(type, settings.GetColor(cell.color));
        }

        private void OnCurrentChanged()
        {
            var level = game.Current;
            Count = level.Count;
            Renat.Log($"OnCurrentChanged {level.Count}");
            data = new Cell[Count, Count];

            Cell.controller = this;
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Count; j++)
                {
                    var value = level[i, j];
                    var cell = new Cell(value, new Vector2Int(i, j));
                    data[i, j] = cell;
                }
            }
        }

        #region cell
        private class Cell
        {
            public static PlaceController controller;
            public int color;
            public readonly Vector2Int position;
            public readonly bool isRoot = false;

            public Cell Top
            {
                get
                {
                    if (controller.TryPosition(position.x, position.y, 0, -1, out int x, out int y))
                    {
                        return controller.data[x, y];
                    }
                    return null;
                }
            }

            public Cell Bottom
            {
                get
                {
                    if (controller.TryPosition(position.x, position.y, 0, +1, out int x, out int y))
                    {
                        return controller.data[x, y];
                    }
                    return null;
                }
            }

            public Cell Left
            {
                get
                {
                    if (controller.TryPosition(position.x, position.y, -1, 0, out int x, out int y))
                    {
                        return controller.data[x, y];
                    }
                    return null;
                }
            }

            public Cell Right
            {
                get
                {
                    if (controller.TryPosition(position.x, position.y, +1, 0, out int x, out int y))
                    {
                        return controller.data[x, y];
                    }
                    return null;
                }
            }

            public bool IsEmpty => !isRoot && color == 0;

            public PlaceChangeDelegate Invoke;

            public Cell(int value, Vector2Int pos)
            {
                color = value;
                position = pos;
                isRoot = value != 0;
            }
        }
        #endregion
    }
}