using ConsoleStorage.Utility;
using System;

namespace Algorithms.Chapter1
{
    public partial class Example3_RoadsMin
    {
        private class GraphStore
        {
            public GraphStore()
            {
            }

            public RectangleGraph Graph { get; internal set; }

            internal void Create()
            {
                int heigth = 0, wight = 0;
                var answer = ConsoleHelper.Query("Введите ширину и длинну через пробел: ").Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (answer.Length < 2 
                    || int.TryParse(answer[0], out wight) 
                    || int.TryParse(answer[1], out heigth))
                {
                    ConsoleHelper.Error("Ошибка ввода!");
                    return;
                }
                RectangleGraph graph = new RectangleGraph(wight, heigth);

                Console.WriteLine("Ведите время перемешения!");
                foreach (var edge in graph.Edges)
                {
                    if (ConsoleHelper.QueryFloat($"{edge}: ", out float time))
                        edge.Time = time;
                    else
                        edge.Time = 1;
                }
                Graph = graph;
                PrintGraph(graph);
            }

            private void PrintGraph(RectangleGraph graph)
            {
                for (int y = 0; y < graph.Heigth; y++)
                {
                    for (int x = 0; x < graph.Wight; x++)
                    {
                        Vertex vertex = graph[x, y];
                        Console.Write(" {0:5} ", vertex);
                    }
                    Console.WriteLine();
                }
            }

            internal void Load()
            {
                throw new NotImplementedException();
            }
        }
    }
}
