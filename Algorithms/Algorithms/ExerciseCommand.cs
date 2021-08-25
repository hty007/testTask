using ConsoleStorage.Command;
using ConsoleStorage.Utility;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Algorithms
{
    public static class ExerciseCommand
    {
        private static Type[] exerciseTypes;

        public static void UseCharterList(ICommandExecutor executor)
        {
            var command = new IntegerCommand("charter ch", PrintExercise);
            command.Discription = "Выводит названия всех упражнений в главе";
            executor.Add(command);
        }

        private static void PrintExercise(int arg)
        {
            foreach (var item in exerciseTypes)
            {
                var exeAttr = Attribute.GetCustomAttribute(item, typeof(ExerciseAttribute)) as ExerciseAttribute;
                if (exeAttr.Chapter == arg)
                {
                    Console.WriteLine($"{exeAttr.Exercise}: {exeAttr.Text}");
                }
            }
        }

        public static void UseExercise(ICommandExecutor executor)
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
            try
            {
                var dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                for (int i = 0; i < 4; i++)
                {
                    dirInfo = dirInfo.Parent;
                }

                var paths = dirInfo.GetFiles("*.dll", SearchOption.AllDirectories).Select(p => p.FullName).ToArray();

                exerciseTypes = ReflectionHelper.GetTypesFromLibraries(paths, typeof(ExerciseAttribute));
            }
            catch
            {
                ConsoleHelper.Error("Ошибка загрузки топов!");
            }
        }

        public static void Exercise(string[] inputs)
        {
            try
            {
                if (inputs.Length >= 2)
                {
                    int exercise = 0, chapter = 0;
                    bool success = int.TryParse(inputs[0], out chapter);
                    success = success && int.TryParse(inputs[1], out exercise);
                    if (success)
                    {
                        foreach (var item in exerciseTypes)
                        {
                            var exeAttr = Attribute.GetCustomAttribute(item, typeof(ExerciseAttribute)) as ExerciseAttribute;

                            if (exeAttr.Chapter == chapter && exeAttr.Exercise == exercise)
                            {
                                PrintTitle(exeAttr);
                                var instanse = Instance(item);
                                var inPro = ReflectionHelper.GetPropertyIsAttributes(item, typeof(InputAttribute));
                                // Вводим с клавиатурв
                                if (inPro.Length > 0)
                                    AskQuestions(instanse, inPro);

                                RunAllMethodsIsAttributes(item, instanse, typeof(InputAttribute));
                                RunAllMethodsIsAttributes(item, instanse, typeof(RunAlgorithmAttribute));
                                RunAllMethodsIsAttributes(item, instanse, typeof(OutputAttribute));

                                var outPro = ReflectionHelper.GetPropertyIsAttributes(item, typeof(OutputAttribute));
                                PrintOutput(instanse, outPro);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.Error($"\n{ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void RunAllMethodsIsAttributes(Type item, object instanse, Type attr)
        {
            var inMeth = ReflectionHelper.GetMetodsIsAttribute(item, attr);
            foreach (var method in inMeth)
            {
                if (method.GetParameters().Length == 0)
                    method.Invoke(instanse, new object[] { });
            }
        }

        private static void PrintOutput(object instanse, PropertyInfo[] outPro)
        {
            foreach (var pro in outPro)
            {
                OutputAttribute outAttr = Attribute.GetCustomAttribute(pro, typeof(OutputAttribute)) as OutputAttribute;

                Console.WriteLine($"{pro.Name} {outAttr.Text}={pro.GetValue(instanse).ToString()}");
            }
        }

        private static object Instance(Type item)
        {
            var ctor = item.GetConstructor(new Type[] { });
            var result = ctor.Invoke(new object[] { });
            return result;
        }

        private static void AskQuestions(object instanse, PropertyInfo[] collectInPro)
        {
            if (collectInPro.Length > 0)
            {
                Console.WriteLine("Заполните входные данные алгоритма");
                foreach (var pro in collectInPro)
                {
                    bool success = false;
                    InputAttribute inAttr = Attribute.GetCustomAttribute(pro, typeof(InputAttribute)) as InputAttribute;

                    // TODO: Не копируй свой код
                    if (pro.PropertyType == typeof(int))
                    {
                        success = ConsoleHelper.QueryInt($"{pro.Name} {inAttr.Text}: ", out int num);
                        if (success)
                        {
                            pro.SetValue(instanse, num);
                        }
                        else
                        {
                            throw new ArgumentNullException($" Ошибка ввода.");
                        }

                    }
                    else if (pro.PropertyType == typeof(float))
                    {
                        success = ConsoleHelper.QueryFloat($"{pro.Name} {inAttr.Text}: ", out float half);
                        if (success)
                        {
                            pro.SetValue(instanse, half);
                        }
                        else
                        {
                            throw new ArgumentNullException($" Ошибка ввода.");
                        }

                    }
                    else if (pro.PropertyType == typeof(string))
                    {
                        var str = ConsoleHelper.Query($"{pro.Name} {inAttr.Text}: ");
                        pro.SetValue(instanse, str);
                    }
                    else
                    {

                        throw new NotImplementedException($" Ввод типа [{pro.PropertyType.Name}] не определён.");
                    }
                }

            }
        }

        private static void PrintTitle(ExerciseAttribute exeAttr)
        {
            Console.WriteLine($"Упражнение {exeAttr.Chapter}.{exeAttr.Exercise} {exeAttr.Text}");
        }
    }
}