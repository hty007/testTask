using UnityEngine;

namespace FlowFree
{
    public class Settings : MonoBehaviour, ISettings
    {
        private readonly Color DEFAULT_COLOR = Color.white;

        [Tooltip("Dropdown для выбора уровня!")]
        [SerializeField]
        private bool selectLavel = true;

        [SerializeField]
        private Color[] colors = null;

        public bool SelectLavel { get => selectLavel; set => selectLavel = value; }

        public Color GetColor(int number)
        {
            var index = number - 1;
            if (colors.Length <= index || index < 0)
                return DEFAULT_COLOR;
            return colors[index];
        }
    }
}
