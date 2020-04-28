using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebTestWork.Models;

/* =============    Тестовое задание    =================

Есть компания, у компании могут быть сотрудники. 
Каждый сотрудник характеризуется именем, датой поступления на работу и базовой ставкой.
Сотрудники бывают 3 видов - Employee, Manager, Sales. У каждого сотрудника может быть начальник. У каждого сотрудника кроме Employee могут быть подчинённые.
Зарплата сотрудника Employee - это базовая ставка плюс 3% за каждый год работы в компании, но не больше 30% суммарной надбавки.
Зарплата сотрудника Manager - это базовая ставка плюс 5% за каждый год работы в компании (но не больше 40% суммарной надбавки за стаж работы) плюс 0,5% зарплаты всех подчинённых первого уровня.
Зарплата сотрудника Sales - это базовая ставка плюс 1% за каждый год работы в компании (но не больше 35% суммарной надбавки за стаж работы) плюс 0,3% зарплаты всех подчинённых всех уровней.
У сотрудников (кроме Employee) может быть любое количество подчинённых любого вида.
Также, для простоты, значение базовой ставки по умолчанию одинаково для всех видов сотрудников.

Нужно реализовать библиотеку с бизнес-логикой, unit тесты и standalone ASP.NET Core приложение. Число проектов в решении не ограничено. 
Одна страничка, которая должна:
- дать возможность получить расчет зарплаты любого сотрудника на произвольный момент времени
- отображать зарплату всех сотрудников
Сохранять/загружать ничего не нужно. 
*/

namespace MSTestWebWork
{
    [TestClass]
    public class RateTest
    {
        private static Company CreaterCompany()
        {
            Company com = new Company();
            com.Add(new Personnel(PersonnelType.Sales) { Name = "Sales1"}); //1
            com.Add(new Personnel(PersonnelType.Sales) { Name = "Sales2"}); //2

            com.Add(new Personnel(PersonnelType.Manager) { Name = "Manager1", IdChief = 1 }); //3
            com.Add(new Personnel(PersonnelType.Manager) { Name = "Manager2", IdChief = 1 }); //4

            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee1", IdChief = 3 }); //5
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee2", IdChief = 3 }); //6
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee3", IdChief = 3 }); //7

            com.Edit(1, basicRate: 100, dateEmployment: new DateTime(2000, 2, 1));
            com.Edit(2, basicRate: 100, dateEmployment: new DateTime(2020, 2, 1));

            com.Edit(3, basicRate: 100, dateEmployment: new DateTime(2020, 2, 1));//101.785
            com.Edit(4, basicRate: 100, dateEmployment: new DateTime(2020, 2, 1));//100

            com.Edit(5, basicRate: 100, dateEmployment: new DateTime(2000, 2, 1)); //130
            com.Edit(6, basicRate: 100, dateEmployment: new DateTime(2020, 2, 1)); //100
            com.Edit(7, basicRate: 100, dateEmployment: new DateTime(2011, 2, 1)); //127   357


            return com;
        }


        [TestMethod]
        public void Employee_0Year()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            Personnel per = com.Personnels.FindTo(x => x.Id == 6);
            // ставка
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate;
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Employee_9Year()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            Personnel per = com.Personnels.FindTo(x => x.Id == 7);
            // ставка+27%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate*(1.27);
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Employee_20Year()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            Personnel per = com.Personnels.FindTo(x => x.Id == 5);
            // ставка+30%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate * (1.3);
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Manager_0Year_0Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            Personnel per = com.Personnels.FindTo(x => x.Id == 4);            
            // ставка
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate;
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Manager_5Year_0Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            com.Edit(4,dateEmployment: DateTime.Now.AddYears(-5));
            Personnel per = com.Personnels.FindTo(x => x.Id == 4);
            // ставка + 5*5%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate*(1.25);
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Manager_9Year_0Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            com.Edit(4, dateEmployment: DateTime.Now.AddYears(-9));
            Personnel per = com.Personnels.FindTo(x => x.Id == 4);
            // ставка + 40%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate * (1.4);
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Manager_0Year_3Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            Personnel per = com.Personnels.FindTo(x => x.Id == 3);
            // ставка + 0% + 357 * 0.5%    
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate * (1) + 357*0.005;
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Manager_5Year_3Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            com.Edit(3, dateEmployment: DateTime.Now.AddYears(-5));
            Personnel per = com.Personnels.FindTo(x => x.Id == 3);
            // ставка + 5*5% + 357 * 0.5%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate * (1.25) + 357 * 0.005;
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Manager_9Year_3Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            com.Edit(3, dateEmployment: DateTime.Now.AddYears(-9));
            Personnel per = com.Personnels.FindTo(x => x.Id == 3);
            // ставка + 40% + 357 * 0.5%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate * (1.4) + 357 * 0.005;
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Sales_0Year_0Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            com.Edit(2, dateEmployment: DateTime.Now);
            Personnel per = com.Personnels.FindTo(x => x.Id == 2);
            // ставка
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate;
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Sales_30Year_0Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            com.Edit(2, dateEmployment: DateTime.Now.AddYears (-30));
            Personnel per = com.Personnels.FindTo(x => x.Id == 2);
            // ставка + 30%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate * (1.3);
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Sales_40Year_0Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            com.Edit(2, dateEmployment: DateTime.Now.AddYears(-40));
            Personnel per = com.Personnels.FindTo(x => x.Id == 2);
            // ставка + 35%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate * (1.35);
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Sales_0Year_5Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            com.Edit(1, dateEmployment: DateTime.Now.AddYears(0));
            Personnel per = com.Personnels.FindTo(x => x.Id == 1);
            // ставка + 0% + 558.785*0.3%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate * (1) + 558.785*0.003;
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Sales_30Year_5Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            com.Edit(1, dateEmployment: DateTime.Now.AddYears(-30));
            Personnel per = com.Personnels.FindTo(x => x.Id == 1);
            // ставка + 30% + 558.785*0.3%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate * (1.3) + 558.785 * 0.003;
            Assert.AreEqual(peyment, actual);
        }

        [TestMethod]
        public void Sales_40Year_5Inferiors()
        {// работник 0 лет выслуги
            Company com = CreaterCompany();
            com.Edit(1, dateEmployment: DateTime.Now.AddYears(-40));
            Personnel per = com.Personnels.FindTo(x => x.Id == 1);
            // ставка + 40% + 558.785*0.3%
            double peyment = per.GetPayment(DateTime.Now);
            double actual = per.BasicRate * (1.35) + 558.785 * 0.003;
            Assert.AreEqual(peyment, actual);
        }

    }
}
