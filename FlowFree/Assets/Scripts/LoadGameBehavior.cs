using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace FlowFree
{
    public class LoadGameBehavior : MonoBehaviour
    {
        [SerializeField]
        private GameObject pleaseHold = null;
        [SerializeField]
        private TMP_Dropdown levelSelector = null;

        private IGameController controller;

        public void SetLavel(int index) => controller.SetLavel(index);

        private IEnumerator LoadLevels()
        {
            Task load = controller.FindLevels();
            yield return new WaitForEndOfFrame();
            pleaseHold.SetActive(true);
            while (!load.IsCompleted)
            {
                yield return new WaitForEndOfFrame();
            }
            controller.SetLavel(0);
            pleaseHold.SetActive(false);
        }

        private void OnLevelLoaded()
        {
            levelSelector.ClearOptions();
            levelSelector.AddOptions(controller.LevelNames.ToList());
        }

        [Inject]
        private void Init(IGameController controller) => this.controller = controller;

        private void Start()
        {
            if (controller == null)
            {
                Debug.Log("controller is null");
                return;
            }
            controller.LevelsLoad += OnLevelLoaded;
            levelSelector.onValueChanged.AddListener(SetLavel);
            StartCoroutine(LoadLevels());
        }
        private void OnDestroy()
        {
            controller.LevelsLoad -= OnLevelLoaded;
            levelSelector.onValueChanged.RemoveListener(SetLavel);
            
        }

        private void Update() => Renat.Update();
    }

    #region Debag Renat
    public static class Renat
    {
        private readonly static Color DEFAULT_COLOR = new Color(1, 1, 1, 1);
        private static System.Collections.Generic.List<RenatLog> autoCollect = new System.Collections.Generic.List<RenatLog>();

        public static string CreateLink(string sourceFilePath, int sourceLineNumber, string content)
        {
            return $"<a href=\"{sourceFilePath}\" line=\"{sourceLineNumber}\">{content}</a>";
        }

        public static void Log(string message, 
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            if (autoCollect.Count == 0)
            {
                var log = CreateLog(memberName, sourceFilePath, sourceLineNumber);
                autoCollect.Add(log);
                log.SetTime();
            }
            autoCollect[0].Add(message, memberName, sourceFilePath, sourceLineNumber);
        }   

        public static void Message(GameObject gameObject = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
            => CreateLog(memberName, sourceFilePath, sourceLineNumber).Send(gameObject);

        public static void Message(string message,
            GameObject gameObject = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var log = CreateLog(memberName, sourceFilePath, sourceLineNumber);
            log.Add(message);
            log.Send(gameObject);
        }

        public static RenatLog CreateLog(
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            return new RenatLog(memberName, sourceFilePath, sourceLineNumber);
        }
        public static RenatLog Auto(
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            var log = new RenatLog(memberName, sourceFilePath, sourceLineNumber);
            log.SetTime(1f);
            autoCollect.Add(log);
            return log;
        }

        public static void Point(Vector3 position, float radius = 5, Color color = default, float second = 5)
        {
            if (color == default)
                color = DEFAULT_COLOR;
            Debug.DrawLine(position + (Vector3.left * radius), position + (Vector3.right * radius), color, second);
            Debug.DrawLine(position + (Vector3.up * radius), position + (Vector3.down * radius), color, second);
            Debug.DrawLine(position + (Vector3.back * radius), position + (Vector3.forward * radius), color, second);
        }

        internal static void Rectangle(RectTransform rectTransform, Color color = default, float second = 5)
        {
            if (color == default)
                color = DEFAULT_COLOR;
            var rect = rectTransform.rect;
            //Debug.DrawLine(rect.min, rect.max, color, 5);
            var a = new Vector2(rectTransform.position.x + rect.min.x, rectTransform.position.y + rect.max.y);
            var b = new Vector2(rectTransform.position.x + rect.min.x, rectTransform.position.y + rect.min.y);
            var c = new Vector2(rectTransform.position.x + rect.max.x, rectTransform.position.y + rect.min.y);
            var d = new Vector2(rectTransform.position.x + rect.max.x, rectTransform.position.y + rect.max.y);

            Debug.DrawLine(a, b, color, second);
            Debug.DrawLine(c, b, color, second);
            Debug.DrawLine(a, d, color, second);
            Debug.DrawLine(c, d, color, second);
        }

        internal static void Update()
        {
            if (autoCollect.Count == 0)
                return;

            if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            {
                foreach (var log in autoCollect)
                {
                    log.Send();
                }

                autoCollect.Clear();
                return;
            }

            foreach (var log in autoCollect.ToArray())
            {
                if (log.Lifetime <= 0)
                {
                    log.Send();
                    autoCollect.Remove(log);
                }
                else
                {
                    log.Tick();
                }
            }
        }

        public class RenatLog
        {
            public RenatLog(string memberName, string sourceFilePath, int sourceLineNumber)
            {
                MemberName = memberName;
                SourceFilePath = sourceFilePath;
                SourceLineNumber = sourceLineNumber;
                builder = new System.Text.StringBuilder($"{MemberName}:\n");
                Lifetime = 9999999f;
            }

            public string MemberName { get; }

            public string SourceFilePath { get; }

            public int SourceLineNumber { get; }

            public float Lifetime { get; private set; }

            private System.Text.StringBuilder builder;

            public void AddTime(float time = 0.5f) => Lifetime += time;

            public void SetTime(float time = 1f) => Lifetime = time;

            public void AddText(string text)
            {
                builder.AppendLine(text);
            }

            public void Add(string message,
                [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
                [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
                [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
            {
                builder.Append(message);
                builder.Append(" | ");
                builder.AppendLine(CreateLink(sourceFilePath, sourceLineNumber, "link"));
            }

            public void Send(GameObject gameObject = null)
            {

                builder.AppendLine($"-----");
                builder.AppendLine(CreateLink(SourceFilePath, SourceLineNumber, "Место вызова"));
                if (!gameObject)
                    UnityEngine.Debug.Log(builder.ToString());
                else
                    UnityEngine.Debug.Log(builder.ToString(), gameObject);
                builder.Clear();
                SetTime(0);
            }

            public void Add(System.Exception ex,
                [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
                [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
                [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
            {
                builder.Append(CreateLink(sourceFilePath, sourceLineNumber, "Ошибка"));
                builder.AppendLine($":");
                builder.AppendLine(ex.Message);
                //builder.AppendLine(ex.StackTrace);

            }

            internal void Property(string key, string value)
            {
                builder.Append(key);
                builder.Append(": ");
                builder.AppendLine(value);
            }

            internal void Space() => builder.AppendLine();

            internal void Property(string key, Vector3 vector) => Property(key, vector.ToString());
            internal void Property(string key, bool value) => Property(key, value.ToString());
            internal void Property(string key, int value) => Property(key, value.ToString());
            internal void Property(string key, double value) => Property(key, value.ToString());
            internal void Property(string key, float value) => Property(key, value.ToString());
            internal void Property(string key, Vector2Int value) => Property(key, value.ToString());

            internal void Send(int count)
            {
                if (builder.Length > count)
                {
                    Send();
                }
            }

            internal void Clear() => builder.Clear();

            internal void Tick() => Lifetime -= Time.deltaTime;
        }
    }

    #endregion

}