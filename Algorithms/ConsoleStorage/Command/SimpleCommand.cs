using System;
using System.Threading.Tasks;

namespace ConsoleStorage.Command
{
    public class SimpleCommand : AConsoleCommand
    {
        private string[] calls;
        private Action action;

        public SimpleCommand(string command, Action action)
        {
            this.calls = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            this.action = action;

        }

        public override string[] Calls => calls;

        public override Task Run(string[] inputs)
        {
            return Task.Run(action);
        }
    }
}
