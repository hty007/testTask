using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlowFree
{
    public class PlaceController : IPlaceController
    {
        private IGameController game;

        private int[,] data;
        private Dictionary<Vector2Int, PlaceChangeDelegate> listeners;

        public PlaceController(IGameController game)
        {
            this.game = game;
            game.CurrentChange += OnCurrentChanged;

            listeners = new Dictionary<Vector2Int, PlaceChangeDelegate>();
        }

        public int Count { get; private set; }

        public void AddListener(Vector2Int pos, PlaceChangeDelegate action) 
            => listeners.Add(pos, action);

        public void Correct(Vector2Int pos, int value)
        {
            if (data == null)
                throw new NullReferenceException();
            if (value == 0)
                return;
            if (data[pos.x, pos.y] == value)
            {
                listeners[pos]?.Invoke(TypeLine.Reset, true);
                return;
            }
            data[pos.x, pos.y] = value;

            Renat.Log($"->Correct: pos {pos}, val {value}");

            if (TryLeftVector(pos, out Vector2Int left))
            {
                Renat.Log($"-->left: {left}");
                var leftValue = data[left.x, left.y];
                if (leftValue == value)
                {
                    SendUpdate(left, TypeLine.Right);
                    SendUpdate(pos, TypeLine.Left);
                }
            }

            if (TryRightVector(pos, out Vector2Int right))
            {
                var rightValue = data[right.x, right.y];
                Renat.Log($"-->right: {right}, rightValue: {rightValue}");
                if (rightValue == value)
                {
                    SendUpdate(right, TypeLine.Left);
                    SendUpdate(pos, TypeLine.Right);
                }
            }

            if (TryTopVector(pos, out Vector2Int top))
            {
                var topValue = data[top.x, top.y];
                Renat.Log($"-->top: {top}, topValue: {topValue}");
                if (topValue == value)
                {
                    SendUpdate(top, TypeLine.Bottom);
                    SendUpdate(pos, TypeLine.Top);
                }
            }

            if (TryBottomVector(pos, out Vector2Int bottom))
            {
                var bottomValue = data[bottom.x, bottom.y];
                Renat.Log($"-->bottom: {bottom}, bottomValue: {bottomValue}");
                if (bottomValue == value)
                {
                    SendUpdate(bottom, TypeLine.Top);
                    SendUpdate(pos, TypeLine.Bottom);
                }
            }
        }

        private bool TryBottomVector(Vector2Int pos, out Vector2Int bottom) => TryVector(pos, 0, +1, out bottom);

        private bool TryTopVector(Vector2Int pos, out Vector2Int top) => TryVector(pos, 0, -1, out top);

        private bool TryRightVector(Vector2Int pos, out Vector2Int right) => TryVector(pos, +1, 0, out right);

        private bool TryLeftVector(Vector2Int pos, out Vector2Int left) => TryVector(pos, -1, 0, out left);

        private bool TryVector(Vector2Int pos, int x, int y, out Vector2Int vector)
        {
            vector = new Vector2Int(pos.x + x, pos.y + y);
            return vector.x >= 0 && vector.x < Count && vector.y >= 0 && vector.y < Count; 
        }

        private void SendUpdate(Vector2Int pos, TypeLine type)
        {
            Renat.Log($"->SendUpdate: {pos}, {type}");
            listeners[pos].Invoke(type, true);
        }

        private void OnCurrentChanged()
        {
            var level = game.Current;
            Count = level.Count;
            data = new int[Count, Count];
            listeners.Clear();

            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Count; j++)
                {
                    data[i, j] = level[i, j];
                }
            }
        }

        public void SetCount(int count)
        {
            OnCurrentChanged();
        }
    }
}