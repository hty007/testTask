using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleStorage.Command
{
    internal class HelpCommand : AConsoleCommand
    {
        private string[] calls = new string[] { "help" };
        private List<AConsoleCommand> commands;

        public HelpCommand(List<AConsoleCommand> commands)
        {
            this.commands = commands;
            Discription = "Получить справку о команде 'help <имя команды>'";
            Help = @"Получить справку о команде или командах
help all            выводит описание о всех командах в системе
help <имя команды>  выводит подробное описание о конкретной команде
help <имя команды> [.. <имя команды>]  выводит подробное описание всех перечисленных команд";
        }

        public override string[] Calls => calls;

        public override Task Run(string[] inputs)
        {
            if (inputs.Length == 0 || inputs.Any(x => x.Equals("all")))
            {
                WriteAll();
            }
            else
            {
                WriteHelp(inputs);
            }
            return Task.CompletedTask;
        }

        private void WriteHelp(string[] inputs)
        {
            foreach (var command in commands.Where(
                com => inputs.Any(
                    input => com.Calls.Contains(input)
                    )
                ))
            {
                Write(command, true);
            }
        }

        private void WriteAll()
        {
            foreach (var command in commands)
            {
                Write(command);
            }
        }

        private void Write(AConsoleCommand command, bool inDetail = false)
        {
            Header(command.Calls[0]);

            if (inDetail)
            {
                Line("    Вызовы команды: ", string.Join(' ', command.Calls));
                Line("    ", command.Help);
            }
            else
                Line("    ", command.Discription);
            Space();
        }
    }
}