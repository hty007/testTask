using UnityEngine;

namespace FlowFree
{
    public delegate void PlaceChangeDelegate(TypeAction line, Color color);

    public interface IPlaceController
    {
        void AddListener(Vector2Int pos, PlaceChangeDelegate action);
        
        void BeginLine(Vector2Int position);
        void EndLine(Vector2Int position);
        void Move(Vector2Int position);
    }
}