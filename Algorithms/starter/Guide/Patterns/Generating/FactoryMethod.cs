using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sterter.Guide.Patterns.Generating
{
    /*
Фабричный метод (Factory Method) - это паттерн, который определяет интерфейс для создания объектов некоторого класса, 
    но непосредственное решение о том, объект какого класса создавать происходит в подклассах. То есть паттерн предполагает, 
    что базовый класс делегирует создание объектов классам-наследникам.
    https://metanit.com/sharp/patterns/2.1.php
Когда надо применять паттерн
    Когда заранее неизвестно, объекты каких типов необходимо создавать
    Когда система должна быть независимой от процесса создания новых объектов и расширяемой: в нее можно легко вводить 
        новые классы, объекты которых система должна создавать.
    Когда создание новых объектов необходимо делегировать из базового класса классам наследникам
Участники
    Абстрактный класс Product определяет интерфейс класса, объекты которого надо создавать.
    Конкретные классы ConcreteProductA и ConcreteProductB представляют реализацию класса Product. 
        Таких классов может быть множество
    Абстрактный класс Creator определяет абстрактный фабричный метод FactoryMethod(), который 
        возвращает объект Product.
    Конкретные классы ConcreteCreatorA и ConcreteCreatorB - наследники класса Creator, определяющие 
        свою реализацию метода FactoryMethod(). 
    Причем метод FactoryMethod() каждого отдельного класса-создателя возвращает определенный конкретный 
        тип продукта. Для каждого конкретного класса продукта определяется свой конкретный класс создателя.
    Таким образом, класс Creator делегирует создание объекта Product своим наследникам. А классы 
        ConcreteCreatorA и ConcreteCreatorB могут самостоятельно выбирать какой конкретный тип продукта им создавать.

    */
    abstract class ProductFM
    { }

    class ConcreteProductA : ProductFM
    { }

    class ConcreteProductB : ProductFM
    { }

    abstract class Creator
    {
        public abstract ProductFM FactoryMethod();
    }

    class ConcreteCreatorA : Creator
    {
        public override ProductFM FactoryMethod() { return new ConcreteProductA(); }
    }

    class ConcreteCreatorB : Creator
    {
        public override ProductFM FactoryMethod() { return new ConcreteProductB(); }
    }
}
