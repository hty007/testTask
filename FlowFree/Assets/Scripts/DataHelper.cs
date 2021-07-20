using System;

namespace FlowFree
{
    public static class DataHelper
    {
        public static Level ParseLevel(string[] inputs)
        {
            int count = inputs.Length;
            Level level = new Level(count);

            for (int y = 0; y < inputs.Length; y++)
            {
                string input = inputs[y];
                string[] cells = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (cells.Length != count)
                {
                    return null;
                }
                for (int x = 0; x < cells.Length; x++)
                {
                    if (int.TryParse(cells[x], out int value))
                    {
                        level[x, y] = value;
                    }
                }
            }

            return level;
        }
    }
}