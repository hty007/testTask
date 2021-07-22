using System;

namespace Algorithms
{
    public class OutputAttribute : Attribute
    {
        public string Text { get; }

        public OutputAttribute(string text)
        {
            Text = text;
        }
    }
}