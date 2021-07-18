using System;

namespace FlowFree
{
    public class Level
    {
        private readonly int count;
        private int[,] cells;

        public Level(int count)
        {
            this.count = count;
            cells = new int[count, count];
        }

        public int this[int i, int j] 
        {
            get
            {
                if (i >= count | j >= count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return cells[i, j];
            }
            set 
            {
                if (i >= count | j >= count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                cells[i, j] = value;
            }
        }

        public string Name { get; internal set; }

        public int Count => count;
    }
}