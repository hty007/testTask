using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
Состояние (State) - шаблон проектирования, который позволяет объекту изменять свое поведение в зависимости от 
    внутреннего состояния.
Когда применяется данный паттерн?
    Когда поведение объекта должно зависеть от его состояния и может изменяться динамически во время выполнения
    Когда в коде методов объекта используются многочисленные условные конструкции, выбор которых зависит от 
        текущего состояния объекта
Участники паттерна
State: определяет интерфейс состояния
Классы StateA и StateB - конкретные реализации состояний
Context: представляет объект, поведение которого должно динамически изменяться в соответствии с состоянием. 
    Выполнение же конкретных действий делегируется объекту состояния

 */
namespace sterter.Guide.Patterns.Behavior
{
    class Program
    {
        static void m_Main()
        {
            Context_State context = new Context_State(new StateA());
            context.Request(); // Переход в состояние StateB
            context.Request();  // Переход в состояние StateA
        }
    }
    abstract class State
    {
        public abstract void Handle(Context_State context);
    }
    class StateA : State
    {
        public override void Handle(Context_State context)
        {
            context.State = new StateB();
        }
    }
    class StateB : State
    {
        public override void Handle(Context_State context)
        {
            context.State = new StateA();
        }
    }

    class Context_State
    {
        public State State { get; set; }
        public Context_State(State state)
        {
            this.State = state;
        }
        public void Request()
        {
            this.State.Handle(this);
        }
    }
}
