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

    public class SimpleCommand<TInput> : AConsoleCommand
    {
        private string[] calls;
        private Action<TInput> action;
        private Func<string[], TInput> convert;

        public SimpleCommand(string command, Action<TInput> action, Func<string[], TInput> convert)
        {
            this.calls = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            this.action = action;
            this.convert = convert;
        }

        public override string[] Calls => calls;

        public override Task Run(string[] inputs)
        {
            if (inputs.Length > 0)
            {
                return Task.Run(() => action.Invoke(convert.Invoke(inputs)));
            }
            return Task.CompletedTask;
        }
    }
}
