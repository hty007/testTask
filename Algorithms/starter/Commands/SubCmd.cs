using System;
using System.Collections.Generic;

namespace sterter

{
    public class SubCmd
    {
        public string Hеader;
        private bool work;
        private List<SubCommand> Commands = new List<SubCommand>();

        public void Execute(params string[] pars)
        {
            Console.WriteLine($"\t===== {Hеader} =====");
            work = true;
            while (work)
            {
                Console.Write($"{Hеader}. Введите команду:");
                string str = Console.ReadLine();
                var a_str = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (a_str.Length == 0) continue;
                if (a_str[0] == "help") { WriteHelp(); continue; }
                if (a_str[0] == "h") { WriteHelp(); continue; }
                if (a_str[0] == "exit") { work = false; continue; }
                if (a_str[0] == "q") { work = false; continue; }
                foreach (SubCommand cmd in Commands)
                {
                    if (cmd.name == a_str[0])
                    {
                        try
                        {
                            cmd.action.Invoke(str.Replace(cmd.name, ""));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Во время выполнения этой команды, возникла ощибка.");
                            //throw;
                        }
                    }
                }
                Console.WriteLine("");

            }
        }

        private void WriteHelp()
        {
            Console.WriteLine($"Команда {Hеader}");
            foreach (var cmd in Commands)
            {
                Console.WriteLine("");
                Console.WriteLine("\t"+cmd.name);                
                Console.Write("\t");
                Console.WriteLine(cmd.help);
            }
        }

        public void AddSubCommand(string name, string help, Action<string> action)
        {
            SubCommand cmd = new SubCommand(name, help, action);
            Commands.Add(cmd);
        }
        internal class SubCommand
        {
            public string name;
            public string help;
            public Action<string> action;

            public SubCommand(string name, string help, Action<string> action)
            {
                this.name = name;
                this.help = help;
                this.action = action;
            }
        }
    }

}
