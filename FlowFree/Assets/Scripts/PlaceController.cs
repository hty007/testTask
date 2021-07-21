using System;
using System.Collections.Generic;
using System.Linq;
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
            // Начало движения
            var cell = data[pos.x, pos.y];
            // Ячейка должнеа быть закрашена
            if (!cell.IsEmpty)
            { // Ячейка не пустая можно двигатся
                Renat.Log($"===>BeginLine: {cell}");
                canMove = true;
                if (!lines.ContainsKey(cell.Color))
                {
                    lines.Add(cell.Color, new Stack<Cell>());
                }
                Renat.Log($"lines.Count: {lines.Count}");

                line = lines[cell.Color];
                if (cell.isRoot)
                {
                    Renat.Log($"cell.isRoot: {cell}");
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
                        line.Clear();
                    }
                    Renat.Log($"line.Count: {line.Count}");
                }
                //else
                //{
                //    Debug.Log("Продолжение движения?");
                //}
            }
            //else
            //{
            //    Renat.Log("Ячейка пустая");
            //}
        }

        public void EndLine(Vector2Int pos)
        {
            Renat.Log($"===>EndLine {pos}");
            canMove = false;

            Renat.Log($"count: {line.Count}");
        }

        public void Move(Vector2Int pos)
        {
            // Нажатый указатель попал в ячейку
            if (!canMove)
                return;

            var cell = data[pos.x, pos.y];
            Renat.Log($"===>Move {cell}");

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
            Renat.Log($"back: {back}");
            if (Vector2Int.Distance(cell.position, back.position) != 1)
            {
                // Движение произошло по диоганали
                return;
            }
            else if (cell.IsEmpty)// Пустая ячейка
            {
                AddCell(cell, back);
            }
            else if (!cell.IsEmpty)// Заполненая ячейка
            {
                if (back.Color == cell.Color) // Цвет совподает
                {
                    var color = back.Color;
                    bool isOld = line.Contains(cell);
                    if (isOld)
                    { 
                        Renat.Log($"isOld:  {cell}");
                        DoEmpty(cell);

                        var empty = line.Pop();
                        while (empty != cell)
                        {
                            DoEmpty(empty);                            
                            empty = line.Pop();
                            if (line.Count == 0)
                                break;
                        }

                        cell.Color = color;
                        if (line.Count > 0)
                        {
                            back = line.Peek();
                            Connect(cell, back);
                            Renat.Log($"line.Count {line.Count}");
                        }

                        line.Push(cell);

                        Renat.Log($"line.Count {line.Count}");
                    }
                    else 
                    {
                        //Цель достигнута

                        Renat.Log($"Finish");
                        AddCell(cell, back);
                    }
                }
                else// Цвет не совподает
                {
                    if (cell.isRoot)
                    {
                        Renat.Log($"isRoot not color");
                        foreach (var item in line)
                        {
                            DoEmpty(item);
                        }
                        canMove = false;
                    }
                    else
                    {
                        Renat.Log($" not color");
                        var line2 = lines[cell.Color];

                        var empty = line2.Pop();
                        while (empty != cell)
                        {
                            DoEmpty(empty);
                            empty = line2.Pop();
                            Renat.Log($"Pop empty: {empty}");
                            if (line2.Count == 1)
                                break;
                        }

                        Cell back2 = null;
                        foreach (var item in line2.Reverse())
                        {
                            DoEmpty(item);
                            if (back2 == null)
                            {
                                back2 = item;
                            }
                            else
                            {
                                item.Color = back2.Color;
                                Connect(back2, item);
                                back2 = item;
                            }
                        }


                        // Todo:  line2 [c]x[b]-[b]
                        DoEmpty(cell);
                        AddCell(cell, back);
                        //cell.Color = back.Color;
                        //Connect(cell, back);
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
            cell.Color = back.Color;
            Connect(cell, back);
        }

        private void DoEmpty(Cell empty)
        {
            empty.Color = 0;
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
            cell.Invoke(type, settings.GetColor(cell.Color));
        }

        private void OnCurrentChanged()
        {
            lines.Clear();
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
            private int color;
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

            public bool IsEmpty => !isRoot && Color == 0;

            public int Color 
            {
                get => color;
                set { 
                    if (!isRoot) 
                        color = value; 
                } 
            }

            public PlaceChangeDelegate Invoke;

            public Cell(int value, Vector2Int pos)
            {
                Color = value;
                position = pos;
                isRoot = value != 0;
            }

            public override string ToString()
            {
                if (isRoot)
                    return $"root{position}[{color}]";
                return $"{position}[{color}]";
            }
        }
        #endregion
    }
}