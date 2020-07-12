using System;
using System.Collections.Generic;

namespace sterter

{
    public class SubCmd
    {
        public string Hеader;
        private bool work;
        private List<SubCommand> Commands;

        public void Execute(params string[] pars)
        {
            Console.WriteLine($"\t===== {Hеader} =====");

            while (work)
            {
                Console.Write("Введите команду:");
                string str = Console.ReadLine();
            }
        }

        public void AddSubCommand(string name, string help, Action action)
        {
            SubCommand cmd = new SubCommand(name, help, action);
            Commands.Add(cmd);
        }
    internal class SubCommand
    {
        private string name;
        private string help;
        private Action action;

        public SubCommand(string name, string help, Action action)
        {
            this.name = name;
            this.help = help;
            this.action = action;
        }
    }
    }

}
