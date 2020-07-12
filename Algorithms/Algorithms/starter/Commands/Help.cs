using System;

namespace starter
{
    public class Help : ICmd
    {
        public void Execute(params string[] pars)
        {
            Console.WriteLine("Программа Starter");
            foreach (var cmd in Program.Commands)
            {
                Console.WriteLine("");
                WriteNames(cmd.GetName());
                Console.Write("\t");
                Console.WriteLine(cmd.GetHelp());
            }
        }

        private void WriteNames(string[] names)
        {
            Console.Write("\t");
            foreach (var name in names)
            {
                Console.Write(name.ToUpper());
                Console.Write("   ");
            }
            Console.WriteLine("");
        }

        public string GetHelp()
        {
            return "Веречень всех команд приложения ";
            //    ", или команд переданных в параметрах";
        }

        public string[] GetName()
        {
            return new[]
            {
                "help",
                "h",
                "помощь"
            };
        }
    }
}
