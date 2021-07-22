using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace starter
{
    class Program
    {
        public static bool work;
        public static List<ICmd> Commands = new List<ICmd>();

        static void Main(string[] args)
        {
            #region Загрузка всех команд
            // Все классы реализующие интерфейс в этой сборке будут загружены в Commands
            Type icmd = typeof(ICmd);
            foreach (Type cl in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (icmd.IsAssignableFrom(cl))
                {
                    try
                    {
                        ConstructorInfo ctor = cl.GetConstructor(new Type[] { });
                        ICmd _cmd = (ICmd)ctor.Invoke(new object[] { });
                        if (_cmd != null)
                        {
                            Commands.Add(_cmd);
                        }
                    }
                    catch { }
                }
            } 
            #endregion
            string str = "";
            work = true;
            Console.Title = "Starter";


            while (work)
            {
                Console.Write("Введите команду:");
                str = Console.ReadLine();
                var a_str = str.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                if (a_str.Length == 0) continue;
                
                foreach (var cmd in Commands)
                {
                    if (cmd.GetName().Contains(a_str[0].ToLower()))
                    {
                        cmd.Execute(a_str);
                    }
                }                
            }
        }
        
    }
}
