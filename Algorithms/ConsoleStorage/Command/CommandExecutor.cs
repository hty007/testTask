using ConsoleStorage.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleStorage.Command
{
    public class CommandExecutor : AConsoleWriter, ICommandExecutor
    {
        private bool work = true;
        protected List<AConsoleCommand> commands = new List<AConsoleCommand>();

        public CommandExecutor()
        {
            commands.Add(new HelpCommand(commands));
            commands.Add(new ListCommand(commands));
        }

        public async Task Run(string[] args)
        {
            if (args.Length > 0)
            {
                await RunCommand(args);
            }
            else
            {
                work = true;
                while (work)
                {
                    string[] words = ConsoleHelper.Query("Введите команду: ").ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    await RunCommand(words);
                }
            }
        }

        private async Task RunCommand(string[] words)
        {
            if (TryParseCommand(words, out string textCommand, out string[] inputs))
            {
                var command = commands.Find(com => com.NeedThisCommand(textCommand));
                if (command != null)
                {
                    try
                    {
                        await command.Run(inputs);
                    }
                    catch (Exception ex)
                    {
                        Error($"Ошибка при выполнении команды [{textCommand}]");
                        Line($"[{ex.GetType().Name}]: {ex.Message}\n{ex.StackTrace}");
                    }
                }
            }
        }

        public void Add(AConsoleCommand command) => commands.Add(command);
        public void AddRange(params AConsoleCommand[] commands) => this.commands.AddRange(commands);

        public Task Stop()
        {
            work = false;
            return Task.CompletedTask;
        }

        private bool TryParseCommand(string[] words, out string command, out string[] inputs)
        {
            if (words.Length > 0)
            {
                command = words[0];
                inputs = words.Skip(1).ToArray();
                return true;
            }

            command = string.Empty;
            inputs = null;
            return false;
        }
    }
}