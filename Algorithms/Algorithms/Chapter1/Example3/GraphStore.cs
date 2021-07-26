using Algorithms.Models;
using ConsoleStorage.Command;
using ConsoleStorage.INI;
using ConsoleStorage.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Algorithms.Chapter1
{
    public partial class Example3_RoadsMin
    {
        private class GraphStore : AConsoleWriter
        {
            private readonly static string STORE_NAME_DIR = "Store";
            private readonly static string FILE_EXTENSION = ".graph";
            public GraphStore()
            {
                var info = Directory.CreateDirectory(STORE_NAME_DIR);
                PathStore = info.FullName;
                graphs = info.GetFiles().Where(f => f.Extension == FILE_EXTENSION).Select(f => f.Name).ToList();
            }

            public RectangleGraph Graph { get; internal set; }
            public string PathStore { get; }

            private List<string> graphs;

            public void Create()
            {
                int heigth = 0, wight = 0;
                var answer = ConsoleHelper.Query("Введите ширину и длинну через пробел: ").Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (answer.Length < 2
                    || !int.TryParse(answer[0], out wight)
                    || !int.TryParse(answer[1], out heigth))
                {
                    throw new Exception("Ошибка ввода!");
                }
                RectangleGraph graph = new RectangleGraph(wight, heigth);
                PrintGraph(graph);

                Console.WriteLine("Ведите время перемешения (-1 проити не возможно)!");
                foreach (Edge edge in graph.Edges)
                {
                    if (ConsoleHelper.QueryFloat($"{edge.Vertex1.Id}=>{edge.Vertex2.Id}: ", out float time))
                        edge.Time = time;
                    else
                        edge.Time = -1;
                }
                Graph = graph;
                SaveGraph(Graph);
            }

            internal void Edit()
            {
                bool edit = true;
                while (edit)
                {
                    try
                    {
                        Clear();
                        Header("Редактор графов");
                        Line($"Объект {Graph.FileName}");
                        PrintGraph(Graph);
                        if (ConsoleHelper.QueryInt("Введите номер вершины для редактирования (выход - отрицательная величина): ", out int id))
                        {
                            if (id < 0)
                                edit = false;

                            var vertex = Graph.Vertices.First(v => v.Id == id);
                            PrintVertex(vertex);
                            if (ConsoleHelper.QueryInt("Введите id ребра для редактирования (выход - отрицательная величина): ", out id))
                            {
                                var edge = vertex.Edges.First(ed => ed.Id == id);


                                if (ConsoleHelper.QueryFloat($"Введите время движения [{edge.Vertex1.Id}=>{edge.Vertex2.Id}]: ", out float newTime))
                                {
                                    edge.Time = newTime;
                                }
                            }
                        }
                    }
                    catch 
                    {
                        continue;
                    }
                }

                int answer = ConsoleHelper.SelectItem("Сохранить этот граф?", "Да", "Нет");
                if (answer == 0)
                    SaveGraph(Graph);

            }

            private void PrintVertex(Vertex vertex)
            {
                #region Старьё
                //var pos = Console.GetCursorPosition();

                //#region Top
                //if (Graph.ExistVertex(vertex.X, vertex.Y - 1))
                //{
                //    Console.SetCursorPosition(10, pos.Top);
                //    var top = Graph.GetVertex(vertex.X, vertex.Y - 1);
                //    Accent($"{top.Id,3}");

                //    var e_top = top.Edges.Where(ed => ed.Vertex1 == vertex).First() as Edge;
                //    Console.SetCursorPosition(7, pos.Top + 2);
                //    Console.Write(e_top.Id);
                //    if (e_top.Time >= 0)
                //    {
                //        Console.SetCursorPosition(10, pos.Top + 1);
                //        Console.Write("↑");
                //        Console.SetCursorPosition(8, pos.Top + 2);
                //        Console.Write(e_top.Time);
                //        Console.SetCursorPosition(10, pos.Top + 3);
                //        Console.Write("↑");
                //    }
                //    var e_buttom = top.Edges.Where(ed => ed.Vertex2 == vertex).First() as Edge;
                //    Console.SetCursorPosition(14, pos.Top + 2);
                //    Console.Write(e_buttom.Id);
                //    if (e_buttom.Time >= 0)
                //    {
                //        Console.SetCursorPosition(12, pos.Top + 1);
                //        Console.Write("↓");
                //        Console.SetCursorPosition(12, pos.Top + 2);
                //        Console.Write(e_buttom.Time);
                //        Console.SetCursorPosition(12, pos.Top + 3);
                //        Console.Write("↓");
                //    }
                //}
                //#endregion
                //#region left
                //if (Graph.ExistVertex(vertex.X - 1, vertex.Y))
                //{
                //    Console.SetCursorPosition(0, pos.Top + 5);
                //    var left = Graph.GetVertex(vertex.X - 1, vertex.Y);
                //    Accent($"{left.Id}");

                //    var e_left = left.Edges.Where(ed => ed.Vertex1 == vertex).First() as Edge;
                //    Console.SetCursorPosition(3, pos.Top + 3);
                //    Console.Write(e_left.Id);
                //    if (e_left.Time >= 0)
                //    {
                //        Console.SetCursorPosition(4, pos.Top + 4);
                //        Console.Write("⬋");
                //        Console.Write(e_left.Time);
                //        Console.SetCursorPosition(8, pos.Top + 4);
                //        Console.Write("⬉");
                //    }
                //    var e_rigth = left.Edges.Where(ed => ed.Vertex2 == vertex).First() as Edge;
                //    Console.SetCursorPosition(4, pos.Top + 6);
                //    Console.Write(e_rigth.Id);
                //    if (e_rigth.Time >= 0)
                //    {
                //        Console.SetCursorPosition(4, pos.Top + 5);
                //        Console.Write("→");
                //        Console.Write(e_rigth.Time);
                //        Console.SetCursorPosition(8, pos.Top + 5);
                //        Console.Write("→");
                //    }
                //}
                //#endregion
                //#region rigth
                //if (Graph.ExistVertex(vertex.X + 1, vertex.Y))
                //{
                //    Console.SetCursorPosition(19, pos.Top + 5);
                //    var left = Graph.GetVertex(vertex.X + 1, vertex.Y);
                //    Accent($"{left.Id}");

                //    var e_left = left.Edges.Where(ed => ed.Vertex2 == vertex).First() as Edge;
                //    Console.SetCursorPosition(18, pos.Top + 3);
                //    Console.Write(e_left.Id);
                //    if (e_left.Time >= 0)
                //    {
                //        Console.SetCursorPosition(13, pos.Top + 4);
                //        Console.Write("⬈");
                //        Console.SetCursorPosition(14, pos.Top + 4);
                //        Console.Write(e_left.Time);
                //        Console.SetCursorPosition(18, pos.Top + 4);
                //        Console.Write("⬊");
                //    }
                //    var e_rigth = left.Edges.Where(ed => ed.Vertex1 == vertex).First() as Edge;
                //    Console.SetCursorPosition(17, pos.Top + 6);
                //    Console.Write(e_rigth.Id);
                //    if (e_rigth.Time >= 0)
                //    {
                //        Console.SetCursorPosition(13, pos.Top + 5);
                //        Console.Write("←");
                //        Console.Write(e_rigth.Time);
                //        Console.SetCursorPosition(18, pos.Top + 5);
                //        Console.Write("←");
                //    }
                //}
                //#endregion
                //#region Buttom
                //if (Graph.ExistVertex(vertex.X, vertex.Y + 1))
                //{
                //    Console.SetCursorPosition(10, pos.Top + 9);
                //    var buttom = Graph.GetVertex(vertex.X, vertex.Y + 1);
                //    Accent($"{buttom.Id,3}");

                //    var e_top = buttom.Edges.Where(ed => ed.Vertex1 == vertex).First() as Edge;
                //    Console.SetCursorPosition(14, pos.Top + 8);
                //    Console.Write(e_top.Id);
                //    if (e_top.Time >= 0)
                //    {
                //        Console.SetCursorPosition(10, pos.Top + 8);
                //        Console.Write("↑");
                //        Console.SetCursorPosition(8, pos.Top + 7);
                //        Console.Write(e_top.Time);
                //        Console.SetCursorPosition(10, pos.Top + 6);
                //        Console.Write("↑");
                //    }
                //    var e_buttom = buttom.Edges.Where(ed => ed.Vertex2 == vertex).First() as Edge;
                //    Console.SetCursorPosition(6, pos.Top + 8);
                //    Console.Write(e_buttom.Id);
                //    if (e_buttom.Time >= 0)
                //    {
                //        Console.SetCursorPosition(12, pos.Top + 8);
                //        Console.Write("↓");
                //        Console.SetCursorPosition(7, pos.Top + 7);
                //        Console.Write(e_buttom.Time);
                //        Console.SetCursorPosition(12, pos.Top + 6);
                //        Console.Write("↓");
                //    }
                //}
                //#endregion
                //Console.SetCursorPosition(10, pos.Top + 5);
                //Accent($"{vertex.Id,3}");
                //Console.SetCursorPosition(0, pos.Top + 10); 
                #endregion

                foreach (Edge edge in vertex.Edges)
                {
                    Console.Write("    ");
                    if (edge.Vertex1 == vertex)
                    {
                        Console.Write("->");
                        Console.Write(edge.Vertex2.Id);
                    }
                    else if (edge.Vertex2 == vertex)
                    {
                        Console.Write("<-");
                        Console.Write(edge.Vertex1.Id);
                    }
                    else
                        Console.Write("?");

                    Console.Write(" id:");
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write(edge.Id);
                    Console.ResetColor();

                    Console.Write($"  {edge.Time}");
                    Console.WriteLine();
                }
            }

            private void Accent(string text)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.WriteLine(text);
                Console.ResetColor();
            }

            public void Load()
            {
                int index = ConsoleHelper.SelectItem("Выберите один из созданых графов:", graphs.ToArray());
                var fileName = Path.Combine(PathStore, graphs[index]);
                RectangleGraph graph = LoadGraph(fileName);
                graph.FileName = fileName;
                Graph = graph;
            }

            private RectangleGraph LoadGraph(string fileName)
            {
                var ini = IniData.LoadFile(fileName);
                var main = ini.GetGroup("Main");
                var wight = (int)main.GetNumeric("Wight");
                var heigth = (int)main.GetNumeric("Heigth");

                RectangleGraph graph = new RectangleGraph(wight, heigth);

                foreach (var group in ini.Groups)
                {
                    if (group.Name.StartsWith("Vertex"))
                    {
                        var idText = group.Name.Replace("Vertex", "");
                        if (int.TryParse(idText, out int id))
                        {
                            var vertex = graph.Vertices.First(v => ((Vertex)v).Id == id) as Vertex;
                            vertex.X = group.GetInteger("X");
                            vertex.Y = group.GetInteger("Y");
                        }
                    }
                    else if (group.Name.StartsWith("Edge"))
                    {
                        var idText = group.Name.Replace("Edge", "");
                        if (int.TryParse(idText, out int id))
                        {
                            var edge = graph.Edges.First(e => ((Edge)e).Id == id) as Edge;
                            edge.Time = group.GetFloat("Time");

                            var id1 = group.GetInteger("Vertex1");
                            if (edge.Vertex1.Id != id1)
                            {
                                AVertex.Disconnect(edge.Vertex1, edge);
                                var vertex1 = graph.Vertices.First(v => ((Vertex)v).Id == id1) as Vertex;
                                AVertex.Connect(vertex1, edge);
                            }

                            var id2 = group.GetInteger("Vertex2");
                            if (edge.Vertex2.Id != id2)
                            {
                                AVertex.Disconnect(edge.Vertex2, edge);
                                var vertex2 = graph.Vertices.First(v => ((Vertex)v).Id == id2) as Vertex;
                                AVertex.Connect(vertex2, edge);
                            }
                        }
                    }
                }
                return graph;
            }

            private void SaveGraph(RectangleGraph graph)
            {
                var nameFile = Path.Combine(PathStore, $"graph-{graphs.Count + 1}{FILE_EXTENSION}");
                var ini = IniData.LoadFile(nameFile);
                var main = ini.GetGroup("Main");

                main.SetNumeric("Wight", graph.Wight, "Ширина графа");
                main.SetNumeric("Heigth", graph.Heigth, "Высота графа");
                main.SetNumeric("Vertices", graph.Vertices.Count, "Количество вершин");
                main.SetNumeric("Edges", graph.Edges.Count, "Количество ребер");

                foreach (Vertex vertex in graph.Vertices)
                {
                    var group = ini.GetGroup($"Vertex {vertex.Id}");
                    group.SetNumeric("X", vertex.X);
                    group.SetNumeric("Y", vertex.Y);
                }

                foreach (Edge edge in graph.Edges)
                {
                    var group = ini.GetGroup($"Edge {edge.Id}");
                    group.SetNumeric("Vertex1", edge.Vertex1.Id);
                    group.SetNumeric("Vertex2", edge.Vertex2.Id);
                    group.SetReal("Time", edge.Time);
                }

                ini.SaveFile();
            }

            public static void PrintGraph(RectangleGraph graph, bool printEdges = false)
            {
                for (int y = 0; y < graph.Heigth; y++)
                {
                    for (int x = 0; x < graph.Wight; x++)
                    {
                        Vertex vertex = graph[x, y];
                        Console.Write(" {0,3}", vertex.Id);
                        if (printEdges)
                            foreach (Edge edge in vertex.Edges)
                            {
                                if (edge.Vertex1.ItYou(x + 1, y) || edge.Vertex2.ItYou(x + 1, y))
                                {
                                    Console.Write(" -{0,3}-", edge.Time);
                                }
                            }
                    }
                    Console.WriteLine();
                    if (printEdges)
                    {
                        for (int x = 0; x < graph.Wight; x++)
                        {
                            Vertex vertex = graph[x, y];
                            Console.Write("   ");

                            foreach (Edge edge in vertex.Edges)
                            {
                                if (edge.Vertex1.ItYou(x, y + 1) || edge.Vertex2.ItYou(x, y + 1))
                                {
                                    Console.Write("{0,3}-", edge.Time);
                                }
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
