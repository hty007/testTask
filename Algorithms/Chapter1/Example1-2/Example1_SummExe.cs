using System;
using System.Text;

namespace Algorithms.Chapter1
{
    [Exercise(
        chapter: 1, 
        exercise: 1, 
        text: "Докажите что значение <a+b> может быть больше, чем значение min(a,b).")]
    public class Example1_SummExe
    {
        [Input("Первое значение")]
        public int A { get; set; }

        [Input("Второе значение")]
        public int B { get; set; }

        [Output("Минимальное значение")]
        public StringBuilder Result { get; private set; }


        [RunAlgorithm]
        public void Summ()
        {
            Result = new StringBuilder();

            int min = Math.Min(A, B);
            int max = Math.Max(A, B);

            int find = 0;
            for (int a = min; a <= max; a++)
            {
                for (int b = min; b <= max; b++)
                {
                    if (a + b < Math.Min(a, b))
                    {
                        find++;
                        Result.AppendLine($"---Найдено <{a}> и <{b}>");
                    }
                }
            }
            Result.AppendLine("======");
            if (find == 0) 
                Result.AppendLine("Ничего не найдено");
            else
                Result.AppendLine($"Найдено {find} подходящих элементов.");
        }
    }
}
