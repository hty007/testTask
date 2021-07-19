using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlowFree
{
    public class CellPlace : MonoBehaviour
    {
        public static Settings settings;
        private static bool isMove;
        private static int moveValue;

        //[SerializeField]
        //private Collider2D collider = null;
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

        public int IndexX { get; private set; }
        public int IndexY { get; private set; }
        public int Value { get; private set; }
        public bool IsStatic { get; private set; } = false;

        public void SetPosition(int i, int j)
        {
            IndexX = i;
            IndexY = j;
        }

        public void SetValue(int value)
        {
            Value = value;
            IsStatic = true;
            // controller.Correct(new Vector2Int(IndexX, IndexY), Value);
        }

        public void OnMouseDown()
        {
            if (IsStatic)
            {
                isMove = true;
                moveValue = Value;
            }
        }

        public void OnMouseUp() => isMove = false;

        public void OnMouseEnter()
        {
            if (isMove)
            {
                Renat.Log($"CellPlace -> Enter: {IndexX}, {IndexY}, {Value}");
                Value = moveValue;
                if (!IsStatic)
                    controller.Correct(new Vector2Int(IndexX, IndexY), Value);
            }
        }

        private void Start()
        {
            if (Value != 0)
            {
                center.gameObject.SetActive(true);
                center.color = settings.GetColor(Value);
            }
            else
            { 
                center.gameObject.SetActive(false);
            }
            controller.AddListener(new Vector2Int(IndexX, IndexY), ChengeAciveLine);

        }

        private void ChengeAciveLine(TypeLine line, bool isActive)
        {
            Renat.Log($"---->ChengeAciveLine: {line}, {isActive}");

            switch (line)
            {
                case TypeLine.Top:
                    top.color = settings.GetColor(Value);
                    top.gameObject.SetActive(isActive);
                    break;
                case TypeLine.Right:
                    right.color = settings.GetColor(Value);
                    right.gameObject.SetActive(isActive);
                    break;
                case TypeLine.Bottom:
                    bottom.color = settings.GetColor(Value);
                    bottom.gameObject.SetActive(isActive);
                    break;
                case TypeLine.Left:
                    left.color = settings.GetColor(Value);
                    left.gameObject.SetActive(isActive);
                    break;
                case TypeLine.Reset:

                    top.gameObject.SetActive(false);
                    right.gameObject.SetActive(false);
                    bottom.gameObject.SetActive(false);
                    left.gameObject.SetActive(false);
                    if(!IsStatic)
                        Value = 0;
                    break;
                default:
                    break;
            }
        }
    }
}