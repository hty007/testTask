using Algorithms.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            public int Id { get; set; }

            public bool ItYou(int x, int y) => X == x && Y == y;

            public override string ToString()
            {
                return $"{X},{Y}";
            }

            public override IReadOnlyCollection<Edge> Edges => edges.Select(q=> (Edge)q).ToList();

        }
    }
}
