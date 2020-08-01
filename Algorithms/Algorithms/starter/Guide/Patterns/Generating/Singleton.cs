using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sterter.Guide.Patterns.Generating
{/*
Одиночка (Singleton, Синглтон) - порождающий паттерн, который гарантирует, что для определенного 
        класса будет создан только один объект, а также предоставит к этому объекту точку доступа.

Когда надо использовать Синглтон? 
Когда необходимо, чтобы для класса существовал только один экземпляр
Синглтон позволяет создать объект только при его необходимости. Если объект не нужен, то он не будет создан. 
В этом отличие синглтона от глобальных переменных.

В классе определяется статическая переменная - ссылка на конкретный экземпляр данного объекта 
и приватный конструктор. В статическом методе getInstance() этот конструктор вызывается для создания объекта, 
если, конечно, объект отсутствует и равен null.

https://metanit.com/sharp/patterns/2.3.php
     */
    class Singleton
    {
        private static Singleton instance;

        private Singleton()
        { }

        public static Singleton getInstance()
        {
            if (instance == null)
                instance = new Singleton();
            return instance;
        }
    }
}
