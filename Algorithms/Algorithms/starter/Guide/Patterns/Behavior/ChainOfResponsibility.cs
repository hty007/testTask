using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
Цепочка Обязанностей (Chain of responsibility) - поведенческий шаблон проектирования, который позволяет избежать жесткой 
    привязки отправителя запроса к получателю, позволяя нескольким объектам обработать запрос. Все возможные обработчики 
    запроса образуют цепочку, а сам запрос перемещается по этой цепочке, пока один из ее объектов не обработает запрос. 
    Каждый объект при получении запроса выбирает, либо обработать запрос, либо передать выполнение запроса следующему по 
    цепочке.

Когда применяется цепочка обязанностей?
    Когда имеется более одного объекта, который может обработать определенный запрос
    Когда надо передать запрос на выполнение одному из нескольких объект, точно не определяя, какому именно объекту
    Когда набор объектов задается динамически


 
*/
namespace sterter.Guide.Patterns.Behavior
{
    class Client_Chain
    {
        void Main()
        {
            Handler h1 = new ConcreteHandler1();
            Handler h2 = new ConcreteHandler2();
            h1.Successor = h2;
            h1.HandleRequest(2);
        }
    }
    abstract class Handler
    {
        public Handler Successor { get; set; }
        public abstract void HandleRequest(int condition);
    }

    class ConcreteHandler1 : Handler
    {
        public override void HandleRequest(int condition)
        {
            if (condition == 1)
            {
                // обработка;
            }
            else if (Successor != null)
            {
                Successor.HandleRequest(condition);
            }
        }
    }

    class ConcreteHandler2 : Handler
    {
        public override void HandleRequest(int condition)
        {
            if (condition == 2)
            {
                // обработка;
            }
            else if (Successor != null)
            {
                Successor.HandleRequest(condition);
            }
        }
    }
}
