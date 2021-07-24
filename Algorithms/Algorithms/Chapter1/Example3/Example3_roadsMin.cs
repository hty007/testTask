using ConsoleStorage.Command;
using ConsoleStorage.Utility;
using System;
using System.Text;

namespace Algorithms.Chapter1
{
    [Exercise(
        chapter: 1,
        exercise: 3,
        text: "Начертите сеть дорог с двумя точками а и b, такими, что маршрут между ними, преодолеваемый за кратчайшее время, не является самым коротким.")]
    public partial class Example3_RoadsMin : AConsoleWriter
    {
        private RectangleGraph graph;

        [Input]
        public void InputParams()
        {
            try
            {
                Header(" --- Установка входных параметров --- ");
                Clear();
                int index = ConsoleHelper.SelectItem("Выберите действие: ", "Загрузить ранее созданный граф", "Создать граф");
                GraphStore store = new GraphStore();
                if (index == 0)
                    store.Load();
                else
                    store.Create();

                graph = store.Graph;

                GraphStore.PrintGraph(graph);

                index = ConsoleHelper.SelectItem("Выберите действие: ", "Выполнить алгоритм", "Редактировать граф");
                if (index == 1)
                    store.Edit();

                Clear();
            }
            catch (Exception ex)
            {
                Error($"{ex.GetType().Name}: {ex.Message}");
                Line($"{ex.StackTrace}");
            }
        }


        [Output]
        public void OutputResult()
        {
            Line("Выходные параметры");
        }


        [RunAlgorithm]
        public void Summ()
        {
            Line("Выполняю алгоритм");
        }
    }
}
