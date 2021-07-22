using ConsoleStorage.Command;
using System;
using System.Threading.Tasks;

namespace Algorithms
{

    
    public class ExerciseCommand
    {
        private static Type[] exerciseTypes;

        public static void Use(ICommandExecutor executor)
        {
            var exercise = new ArrayCommand("exe exercise", Exercise);
            exercise.Discription = "Выполняет упражение exercise <Номер главы> <номер задания>";
            // TODO
            exercise.Help = "Подробное описание пока отсутвует";
            executor.Add(exercise);
            FindExercise();
        }

        private static void FindExercise()
        {
            exerciseTypes = ReflectionHelper.GetTypeThisAssembly(typeof(ExerciseAttribute));
        }

        public static void Exercise(string[] inputs)
        {
            if (inputs.Length > 2)
            {
                bool success = int.TryParse(inputs[0], out int chapter);
                success = success && int.TryParse(inputs[1], out int exercise);
                if (success)
                {
                    foreach (var item in exerciseTypes)
                    {
                        var exeAttr = Attribute.GetCustomAttribute(item, typeof(ExerciseAttribute)) as ExerciseAttribute;

                        //ReflectionHelper.GetPropertyIsAttributes(item, typeof(InputAttribute));
                    }
                }

            }
        }
    }
}