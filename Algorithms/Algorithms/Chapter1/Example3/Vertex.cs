using Algorithms.Models;

namespace Algorithms.Chapter1
{
    public partial class Example3_RoadsMin
    {
        private class Vertex : AVertex
        {
            public Vertex(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }

            public override string ToString()
            {
                return $"{X},{Y}";
            }
        }
    }
}
