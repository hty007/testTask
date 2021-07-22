using System.Threading.Tasks;

namespace ConsoleStorage.Command
{
    public interface ICommandExecutor
    {
        void Add(AConsoleCommand command);
        void AddRange(params AConsoleCommand[] commands);
        Task Run(string[] args);
        Task Stop();
    }
}