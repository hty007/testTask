using System;

namespace Algorithms
{
    public class ExerciseAttribute : Attribute
    {
        public ExerciseAttribute(int chapter, int exercise, string text) 
        {
            this.Chapter = chapter;
            this.Exercise = exercise;
            this.Text = text;
        }

        public int Chapter { get; }
        public int Exercise { get; }
        public string Text { get; }
    }
}