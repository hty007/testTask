using System;

namespace FlowFree
{
    public class Level
    {
        private int count;
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
                if (i < count & j < count)
                    return cells[i, j];
                throw new ArgumentOutOfRangeException();
            }
            set 
            {
                if (i < count & j < count)
                    cells[i, j] = value;
                throw new ArgumentOutOfRangeException();
            }
        }

        public string Name { get; internal set; }
    }
}