using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleStorage.Command
{
    public class ListCommand : AConsoleCommand
    {
        private List<AConsoleCommand> commands;
        private string[] calls = new string[] { "commands", "com-ls", "ls-com" };
        public ListCommand(List<AConsoleCommand> commands)
        {
            this.commands = commands;
            Discription = "Вывести все команды";
            Help = @"Вывести все команды";
        }

        public override string[] Calls => calls;

        public override Task Run(string[] inputs)
        {
            foreach (var command in commands)
            {
                Line(command.Calls);
                
            }
            Space();
            return Task.CompletedTask;
        }
    }
}