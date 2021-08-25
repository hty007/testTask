using ConsoleStorage.Command;
using System;

namespace Algorithms
{
    internal class AlgorithmsFactory
    {
        internal static void Use(ICommandExecutor executor)
        {
            ExerciseCommand.UseExercise(executor);
            ExerciseCommand.UseCharterList(executor);

        }
    }
}