using System;

namespace Algorithms
{
    public class InputAttribute : Attribute
    {
        public InputAttribute(string text)
        {
            this.Text = text;
        }
        public InputAttribute()
        {
        }

        public string Text { get; } = string.Empty;
    }
}