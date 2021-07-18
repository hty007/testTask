using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlowFree
{
    public class CellPlace : MonoBehaviour
    {
        public static Settings settings;
        private static bool isMove;

        [SerializeField]
        private Button cellButton = null;
        [SerializeField]
        private Image center = null;
        [SerializeField]
        private Image top = null;
        [SerializeField]
        private Image bottom = null;
        [SerializeField]
        private Image right = null;
        [SerializeField]
        private Image left = null;
        private Renat.RenatLog log;

        public int IndexX { get; private set; }
        public int IndexY { get; private set; }
        public int Value { get; private set; }

        internal void SetPosition(int i, int j)
        {
            IndexX = i;
            IndexY = j;

            if (log == null)
                log = Renat.Auto();
            log.AddText($"{IndexX}, {IndexY}");
        }

        internal void SetValue(int value)
        {
            Value = value;
            center.gameObject.SetActive(true);
            center.color = settings.GetColor(value);
            if (log == null)
                log = Renat.Auto();
            log.Property(nameof(Value), Value);
        }

        // Start is called before the first frame update
        void Start()
        {
            center.gameObject.SetActive(false);
            log = Renat.Auto();
            log.SetTime(5);
        }

        private void OnMouseEnter()
        {
            if (isMove)
            {
                Debug.Log($"{IndexX}, {IndexY}, {Value}");
            }
        }

        private void OnMouseDown() => isMove = true;
        private void OnMouseUp() => isMove = false;
    }
}