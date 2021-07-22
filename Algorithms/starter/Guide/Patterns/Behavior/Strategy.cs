using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
Паттерн Стратегия (Strategy) представляет шаблон проектирования, который определяет набор алгоритмов, инкапсулирует 
    каждый из них и обеспечивает их взаимозаменяемость. В зависимости от ситуации мы можем легко заменить один 
    используемый алгоритм другим. При этом замена алгоритма происходит независимо от объекта, который использует 
    данный алгоритм.
    https://metanit.com/sharp/patterns/3.1.php
Когда использовать стратегию?
    Когда есть несколько родственных классов, которые отличаются поведением. Можно задать один основной класс, а разные 
        варианты поведения вынести в отдельные классы и при необходимости их применять
    Когда необходимо обеспечить выбор из нескольких вариантов алгоритмов, которые можно легко менять в зависимости от условий
    Когда необходимо менять поведение объектов на стадии выполнения программы
    Когда класс, применяющий определенную функциональность, ничего не должен знать о ее реализации

Участники
    Интерфейс IStrategy, который определяет метод Algorithm(). Это общий интерфейс для всех реализующих его алгоритмов. 
        Вместо интерфейса здесь также можно было бы использовать абстрактный класс.
    Классы ConcreteStrategy1 и ConcreteStrategy, которые реализуют интерфейс IStrategy, предоставляя свою версию метода 
        Algorithm(). Подобных классов-реализаций может быть множество.
    Класс Context хранит ссылку на объект IStrategy и связан с интерфейсом IStrategy отношением агрегации.

В данном случае объект IStrategy заключена в свойстве ContextStrategy, хотя также для нее можно было бы определить приватную 
    переменную, а для динамической установки использовать специальный метод.
*/
namespace sterter.Guide.Patterns.Behavior
{
    public interface IStrategy
    {
        void Algorithm();
    }

    public class ConcreteStrategy1 : IStrategy
    {
        public void Algorithm()
        { }
    }

    public class ConcreteStrategy2 : IStrategy
    {
        public void Algorithm()
        { }
    }

    public class Context_Strategy
    {
        public IStrategy ContextStrategy { get; set; }

        public Context_Strategy(IStrategy _strategy)
        {
            ContextStrategy = _strategy;
        }

        public void ExecuteAlgorithm()
        {
            ContextStrategy.Algorithm();
        }
    }
}
