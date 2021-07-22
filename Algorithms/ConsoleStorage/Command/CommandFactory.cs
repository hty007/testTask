using System;

namespace ConsoleStorage.Command
{
    public static class CommandFactory
    {
        public static void UseStop(ICommandExecutor executor)
        {
            var command = new SimpleCommand("quit q exit", () => executor.Stop());
            command.Discription = "Останавливает выполнение программы";
            command.Help = "Программа прекращает свое выполнение";

            executor.Add(command);
        }

        public static void UseClear(ICommandExecutor executor)
        {
            var command = new SimpleCommand("clear clr", () => Console.Clear()) 
            { 
                Discription = "Очищает консоль", 
                Help = "Очищает консоль" 
            };
            executor.Add(command);
        }
    }
}
