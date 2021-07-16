using System;

namespace FlowFree
{
    public static class DataHelper
    {
        public static Level ParseLevel(string[] inputs)
        {
            int count = inputs.Length;
            Level level = new Level(count);

            for (int i = 0; i < inputs.Length; i++)
            {
                string input = inputs[i];
                string[] cells = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (cells.Length != count)
                {
                    return null;
                }
                for (int j = 0; j < cells.Length; j++)
                {
                    if (int.TryParse(cells[j], out int value))
                    {
                        level[i, j] = value;
                    }
                }
            }

            return level;
        }
    }
}