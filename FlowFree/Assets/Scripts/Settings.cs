using UnityEngine;

namespace FlowFree
{
    public class Settings : MonoBehaviour
    {
        private readonly Color DEFAULT_COLOR = Color.white;
        [SerializeField]
        private Color[] colors = null;

        public Color GetColor(int number)
        {
            var index = number - 1;
            if (colors.Length <= index)
                return DEFAULT_COLOR;
            return colors[index];
        }
    }
}
