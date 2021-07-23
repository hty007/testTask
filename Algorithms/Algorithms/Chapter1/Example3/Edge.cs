using Algorithms.Models;

namespace Algorithms.Chapter1
{
    public partial class Example3_RoadsMin
    {
        private class Edge : AEdge
        {

            public Edge(Vertex vertex1, Vertex vertex2)
            {
                AVertex.Connect(vertex1, this);
                AVertex.Connect(vertex2, this);
            }

            public float Time { get; set; }

            public override Vertex Vertex1 => (Vertex)vertex1;
            public override Vertex Vertex2 => (Vertex)vertex2;

            public override string ToString()
            {
                return $"({Vertex1})->({Vertex2})";
            }
        }
    }
}
