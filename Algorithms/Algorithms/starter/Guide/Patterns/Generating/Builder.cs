using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sterter.Guide.Patterns.Generating
{/*
    Когда использовать паттерн Строитель?
       = Когда процесс создания нового объекта не должен зависеть от того, из каких частей этот объект состоит и 
        как эти части связаны между собой

       = Когда необходимо обеспечить получение различных вариаций объекта в процессе его создания
    Участники
        Product: представляет объект, который должен быть создан. В данном случае все части объекта заключены в списке parts.

        Builder: определяет интерфейс для создания различных частей объекта Product

        ConcreteBuilder: конкретная реализация Buildera. Создает объект Product и определяет интерфейс для доступа к нему

        Director: распорядитель - создает объект, используя объекты Builder
     */
    class Director
    {
        Builder builder;
        public Director(Builder builder)
        {
            this.builder = builder;
        }
        public void Construct()
        {
            builder.BuildPartA();
            builder.BuildPartB();
            builder.BuildPartC();
        }
    }

    abstract class Builder
    {
        public abstract void BuildPartA();
        public abstract void BuildPartB();
        public abstract void BuildPartC();
        public abstract ProductBD GetResult();
    }

    class ProductBD
    {
        List<object> parts = new List<object>();
        public void Add(string part)
        {
            parts.Add(part);
        }
    }

    class ConcreteBuilder : Builder
    {
        ProductBD product = new ProductBD();
        public override void BuildPartA()
        {
            product.Add("Part A");
        }
        public override void BuildPartB()
        {
            product.Add("Part B");
        }
        public override void BuildPartC()
        {
            product.Add("Part C");
        }
        public override ProductBD GetResult()
        {
            return product;
        }
    }
}
