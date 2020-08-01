using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sterter.Guide.Patterns.Generating
{
    /*
    Паттерн "Абстрактная фабрика" (Abstract Factory) предоставляет интерфейс для создания семейств 
       взаимосвязанных объектов с определенными интерфейсами без указания конкретных типов данных объектов.

    Когда использовать абстрактную фабрику
        Когда система не должна зависеть от способа создания и компоновки новых объектов
        Когда создаваемые объекты должны использоваться вместе и являются взаимосвязанными
    Паттерн определяет следующих участников:
        Абстрактные классы AbstractProductA и AbstractProductB определяют интерфейс для классов, 
            объекты которых будут создаваться в программе.
        Конкретные классы ProductA1 / ProductA2 и ProductB1 / ProductB2 представляют конкретную 
            реализацию абстрактных классов
        Абстрактный класс фабрики AbstractFactory определяет методы для создания объектов. Причем 
            методы возвращают абстрактные продукты, а не их конкретные реализации.
        Конкретные классы фабрик ConcreteFactory1 и ConcreteFactory2 реализуют абстрактные методы 
            базового класса и непосредственно определяют какие конкретные продукты использовать
        Класс клиента Client использует класс фабрики для создания объектов. При этом он использует 
            исключительно абстрактный класс фабрики AbstractFactory и абстрактные классы продуктов 
            AbstractProductA и AbstractProductB и никак не зависит от их конкретных реализаций
https://metanit.com/sharp/patterns/2.2.php
    */
    abstract class AbstractFactory
    {
        public abstract AbstractProductA CreateProductA();
        public abstract AbstractProductB CreateProductB();
    }
    class ConcreteFactory1 : AbstractFactory
    {
        public override AbstractProductA CreateProductA()
        {
            return new ProductA1();
        }

        public override AbstractProductB CreateProductB()
        {
            return new ProductB1();
        }
    }
    class ConcreteFactory2 : AbstractFactory
    {
        public override AbstractProductA CreateProductA()
        {
            return new ProductA2();
        }

        public override AbstractProductB CreateProductB()
        {
            return new ProductB2();
        }
    }

    abstract class AbstractProductA
    { }

    abstract class AbstractProductB
    { }

    class ProductA1 : AbstractProductA
    { }

    class ProductB1 : AbstractProductB
    { }

    class ProductA2 : AbstractProductA
    { }

    class ProductB2 : AbstractProductB
    { }

    class Client_AbstractFatoty
    {
        private AbstractProductA abstractProductA;
        private AbstractProductB abstractProductB;

        public Client_AbstractFatoty(AbstractFactory factory)
        {
            abstractProductB = factory.CreateProductB();
            abstractProductA = factory.CreateProductA();
        }
        public void Run()
        { }
    }
}
