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
    public class Example3_RoadsMin : AConsoleWriter
    {
        [Input]
        public void InputParams()
        {
            Header(" --- Входные параметры --- ");
            bool success = ConsoleHelper.QueryInt("Ведите ширину графа: ", out int wigth);
            success = success && ConsoleHelper.QueryInt("Ведите высоту графа: ", out int height);



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
