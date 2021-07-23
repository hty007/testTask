using System;

namespace Algorithms.Models
{
    public class AEdge
    {
        protected AVertex vertex1;
        protected AVertex vertex2;

        public virtual AVertex Vertex1 { get => vertex1; }
        public virtual AVertex Vertex2 { get => vertex2; }

        public static bool SetVertex(AEdge edge, AVertex vertex)
        {
            if (edge.vertex1 == null)
            {
                edge.vertex1 = vertex;
            }
            else if (edge.vertex2 == null)
            {
                edge.vertex2 = vertex;
            }
            else
                return false;
            return true;

        }

        public static bool RemoveVertex(AEdge edge, AVertex vertex)
        {
            if (edge.vertex1 == vertex)
            {
                edge.vertex1 = null;
            }
            else if (edge.vertex2 == vertex)
            {
                edge.vertex2 = null;
            }
            else
                return false;
            return true;
        }
    }
}