using ConsoleStorage.Command;
using ConsoleStorage.Utility;
using System.Text;

namespace Algorithms.Chapter1
{
    [Exercise(
        chapter: 1,
        exercise: 3,
        text: "Начертите сеть дорог с двумя точками а и b, такими, что маршрут между ними, преодолеваемый за кратчайшее время, не является самым коротким.")]
    public partial class Example3_RoadsMin : AConsoleWriter
    {
        [Input]
        public void InputParams()
        {
            Header(" --- Установка входных параметров --- ");
            Clear();
            int index = ConsoleHelper.SelectItem("Выберите действие: ", "Загрузить ранее созданный граф", "Создать граф");
            GraphStore store = new GraphStore();
            if (index == 1)
                store.Load();
            else
                store.Create();

            RectangleGraph graph = store.Graph;





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
