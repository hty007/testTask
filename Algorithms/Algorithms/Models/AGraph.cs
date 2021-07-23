using System;
using System.Collections.Generic;

namespace Algorithms.Models
{
    public class AGraph
    {
        protected List<AEdge> edges = new List<AEdge>();
        protected List<AVertex> vertices = new List<AVertex>();

        public virtual IReadOnlyCollection<AEdge> Edges { get => edges; }
        public virtual IReadOnlyCollection<AVertex> Vertices { get => vertices; }

    }
}
