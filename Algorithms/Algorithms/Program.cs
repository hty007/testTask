using ConsoleStorage.Command;
using System;
using System.Threading.Tasks;

namespace Algorithms
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ICommandExecutor executor = new CommandExecutor();

            CommandFactory.UseStop(executor);
            CommandFactory.UseClear(executor);

            ExerciseCommand.Use(executor); 

            await executor.Run(args);
        }
    }
}
