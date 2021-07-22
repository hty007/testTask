using System;

namespace Algorithms
{
    public class InputAttribute : Attribute
    {
        public InputAttribute(string text)
        {
            this.Text = text;
        }

        public string Text { get; }
    }
}