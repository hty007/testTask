using System;

namespace Algorithms
{
    public class OutputAttribute : Attribute
    {

        public OutputAttribute(string text)
        {
            Text = text;
        }

        public OutputAttribute()
        {
        }

        public string Text { get; } = string.Empty;
    }
}