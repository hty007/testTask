using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleStorage.Command
{
    public abstract class AConsoleCommand : AConsoleWriter
    {
        /// <summary>
        /// <para>Вызовы команды</para> 
        /// </summary>
        public abstract string[] Calls { get; }

        /// <summary>Описание команды</summary>
        public virtual string Discription { get; set; }

        /// <summary>Описание команды</summary>
        public virtual string Help { get; set; }

        /// <summary>Выполняется после выбора команды</summary>
        public abstract Task Run(string[] inputs);

        /// <summary>
        /// Подходит ли команда для выполнения
        /// </summary>
        /// <param name="textCommand"></param>
        /// <returns></returns>
        public bool NeedThisCommand(string textCommand)
        {
            return Calls.Any(call => call.Equals(textCommand));
        }
    }
}
