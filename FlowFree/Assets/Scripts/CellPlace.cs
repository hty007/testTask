using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FlowFree
{
    // This view
    public class CellPlace : MonoBehaviour
    {
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
        internal static IPlaceController controller;

        public Vector2Int Position { get; private set; }
        public int IndexY { get; private set; }

        public void SetPosition(Vector2Int pos) => Position = pos;

        public void OnMouseDown() => controller.BeginLine(Position);

        public void OnMouseUp() => controller.EndLine(Position);

        public void OnMouseEnter() => controller.Move(Position);

        private void Start()
        {
            controller.AddListener(Position, RunAction);
        }

        private void RunAction(TypeAction line, Color color)
        {
            // Renat.Log($"RunAction:{Position}, {line}, {color}");

            switch (line)
            {
                case TypeAction.Top:
                    top.color = color;
                    top.gameObject.SetActive(true);
                    break;
                case TypeAction.Right:
                    right.color = color;
                    right.gameObject.SetActive(true);
                    break;
                case TypeAction.Bottom:
                    bottom.color = color;
                    bottom.gameObject.SetActive(true);
                    break;
                case TypeAction.Left:
                    left.color = color;
                    left.gameObject.SetActive(true);
                    break;
                case TypeAction.Center:
                    center.color = color;
                    center.gameObject.SetActive(true);
                    break;
                case TypeAction.Reset:

                    top.gameObject.SetActive(false);
                    right.gameObject.SetActive(false);
                    bottom.gameObject.SetActive(false);
                    left.gameObject.SetActive(false);
                    center.gameObject.SetActive(false);
                    
                    break;
                default:
                    break;
            }
        }
    }
}