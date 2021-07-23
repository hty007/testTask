using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.Models
{
    public class AVertex
    {
        private List<AEdge> edges = new List<AEdge>();

        public IReadOnlyCollection<AEdge> Edges { get => edges; }
        public void AddEdge(AEdge edge)
        {
            Connect(this, edge);
        }

        public static void Connect(AVertex vertex, AEdge edge)
        {
            vertex.edges.Add(edge);
            AEdge.SetVertex(edge, vertex); 
        }

        public static void Disconnect(AVertex vertex, AEdge edge)
        {
            vertex.edges.Remove(edge);
            AEdge.RemoveVertex(edge, vertex);
        }
    }
}
