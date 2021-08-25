using ConsoleStorage.Utility;
using System;
using System.Text;

namespace ConsoleStorage.Command
{
    public abstract class AConsoleWriter
    {
        /// <summary>
        /// Пишет сообщение красными буквами на белом фоне
        /// </summary>
        /// <param name="message">сообщение</param>
        protected void Error(string message) => ConsoleHelper.Error(message);

        /// <summary>
        /// Пишет сообщение черными буквами на белом фоне
        /// </summary>
        /// <param name="message">сообщение</param>
        protected void Header(string header) => ConsoleHelper.Header(header);

        /// <summary>
        /// Пишет сообщение с переносом строки
        /// </summary>
        /// <param name="message">сообщение</param>
        protected void Line(params string[] messages) => Console.WriteLine(string.Join(" ", messages));

        /// <summary>
        /// Пишет сообщение
        /// </summary>
        /// <param name="message">сообщение</param>
        protected void Text(string message) => Console.Write(message);

        /// <summary>
        /// Очищает экран
        /// </summary>
        /// <param name="message">сообщение</param>
        protected void Clear() => Console.Clear();

        /// <summary>
        /// Пропускает строку
        /// </summary>
        /// <param name="message">сообщение</param>
        protected void Space() => Console.WriteLine();
    }
}
