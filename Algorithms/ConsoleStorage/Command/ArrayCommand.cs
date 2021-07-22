using System;
using System.Threading.Tasks;

namespace ConsoleStorage.Command
{
    public class ArrayCommand : AConsoleCommand
    {
        private string[] calls;
        private Action<string[]> action;

        public ArrayCommand(string command, Action<string[]> action)
        {
            this.calls = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            this.action = action;

        }

        public override string[] Calls => calls;

        public override Task Run(string[] inputs)
        {
            return Task.Run(()=>action(inputs));
        }
    }
}
