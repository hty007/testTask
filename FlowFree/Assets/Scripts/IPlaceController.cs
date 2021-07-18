using UnityEngine;

namespace FlowFree
{
    public delegate void PlaceChangeDelegate(TypeLine line, bool isActive);

    public interface IPlaceController
    {
        void AddListener(Vector2Int pos, PlaceChangeDelegate action);

        void Correct(Vector2Int pos, int value);
        /// <summary>
        /// Костыль
        /// </summary>
        /// <param name="count"></param>
        void SetCount(int count);
    }
}