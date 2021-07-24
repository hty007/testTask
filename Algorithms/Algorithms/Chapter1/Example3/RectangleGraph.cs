using Algorithms.Models;
using System;
using System.Collections.Generic;

namespace Algorithms.Chapter1
{
    public partial class Example3_RoadsMin
    {
        private class RectangleGraph : AGraph
        {
            private int wight;
            private int heigth;
            private int INDEX;

            public string FileName { get; internal set; }

            public RectangleGraph(int wight, int heigth)
            {
                this.wight = wight;
                this.heigth = heigth;



                for (int j = 0; j < heigth; j++)
                {
                    for (int i = 0; i < wight; i++)
                    {
                        vertices.Add(new Vertex(i, j) { Id = GetNewId()});
                    }
                }

                foreach (Vertex vertex in vertices)
                {
                    List<(int x, int y)> friends = new List<(int x, int y)>() {
                        (vertex.X + 1, vertex.Y),
                        (vertex.X - 1, vertex.Y),
                        (vertex.X , vertex.Y+1),
                        (vertex.X , vertex.Y-1),
                    };

                    foreach ((int x, int y) in friends)
                    {
                        if (ExistVertex(x, y))
                        {
                            var vertex2 = GetVertex(x, y);

                            CreateEdge(vertex, vertex2);
                        }
                    }
                }
            }

            private int GetNewId() => INDEX++;

            public int Wight => wight;
            public int Heigth => heigth;

            public Vertex this[int x, int y] => GetVertex(x,y);

            private void CreateEdge(Vertex vertex1, Vertex vertex2)
            {
                var edge = new Edge(vertex1, vertex2);
                edge.Id = GetNewId();
                edges.Add(edge);
            }

            public Vertex GetVertex(int i, int j)
            {
                int index = wight * j + i;
                return (Vertex)vertices[index];
            }

            public bool ExistVertex(int i, int j)
            {
                return i >= 0 && i < wight && j >= 0 && j < heigth;
            }

            //public override IReadOnlyCollection<Vertex> Vertices => (IReadOnlyCollection<Vertex>)vertices;
            //public override IReadOnlyCollection<Edge> Edges => edges;

        }
    }
}
