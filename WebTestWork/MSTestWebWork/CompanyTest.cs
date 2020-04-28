using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebTestWork.Models;

namespace MSTestWebWork
{
    [TestClass]
    public class CompanyTest
    {
        private static Company CreaterCompany()
        {
            Company com = new Company();
            com.Add(new Personnel(PersonnelType.Sales) { Name = "Sales1" }); //1
            com.Add(new Personnel(PersonnelType.Sales) { Name = "Sales2" }); //2

            com.Add(new Personnel(PersonnelType.Manager) { Name = "Manager1", IdChief = 1 }); //3
            com.Add(new Personnel(PersonnelType.Manager) { Name = "Manager2", IdChief = 1 }); //4
            com.Add(new Personnel(PersonnelType.Manager) { Name = "Manager3", IdChief = 2 }); //5
            com.Add(new Personnel(PersonnelType.Manager) { Name = "Manager4", IdChief = 2 }); //6

            com.Add(new Personnel(PersonnelType.Manager) { Name = "Manager5", IdChief = 3 }); //7

            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee1", IdChief = 3 }); //8
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee2", IdChief = 3 }); //9
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee3", IdChief = 3 }); //10
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee4", IdChief = 3 }); //11
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee5", IdChief = 4 }); //12
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee6", IdChief = 4 }); //13
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee7", IdChief = 5 }); //14
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee8", IdChief = 5 }); //15
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee9", IdChief = 6 }); //16
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee10", IdChief = 6 }); //17
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee11", IdChief = 6 }); //18
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee12", IdChief = 7 }); //19
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee13", IdChief = 7 }); //20
            com.Add(new Personnel(PersonnelType.Employee) { Name = "Employee14", IdChief = 7 }); //21
            return com;
        }

        [TestMethod]
        public void AddPersonSample1()
        {// Проверка подчиненых
            Company com = CreaterCompany();

            Assert.AreEqual(com.Personnels.Count, 21);

            Personnel Sales1 = com.Personnels.FindTo(x => x.Id == 1);

            Assert.AreEqual(Sales1.Inferiors.Count, 2);

            Assert.AreEqual(Sales1.GetCountInferiors(1), 2);//Первого уровня
            Assert.AreEqual(Sales1.GetCountInferiors(2), 9);// Второго уровня
            Assert.AreEqual(Sales1.GetCountInferiors(-1), 12);// Всех уровней

        }

        [TestMethod]
        public void AddPersonSample2()
        {
            Company com = CreaterCompany();
            Personnel Sales2 = com.Personnels.FindTo(x => x.Id == 2);

            Assert.AreEqual(Sales2.Inferiors.Count, 2);

            Assert.AreEqual(Sales2.GetCountInferiors(1), 2);
            Assert.AreEqual(Sales2.GetCountInferiors(2), 7);
            Assert.AreEqual(Sales2.GetCountInferiors(-1), 7);
        }

        [TestMethod]
        public void AddPersonSample3()
        {
            Company com = CreaterCompany();
            Personnel Manager = com.Personnels.FindTo(x => x.Id == 3);

            Assert.AreEqual(Manager.Inferiors.Count, 5);

            Assert.AreEqual(Manager.GetCountInferiors(1), 5);
            Assert.AreEqual(Manager.GetCountInferiors(2), 8);
            Assert.AreEqual(Manager.GetCountInferiors(-1), 8);
        }

        [TestMethod]
        public void AddPersonSample4()
        {
            Company com = CreaterCompany();
            Personnel Manager = com.Personnels.FindTo(x => x.Id == 4);

            Assert.AreEqual(Manager.Inferiors.Count, 2);

            Assert.AreEqual(Manager.GetCountInferiors(1), 2);
            Assert.AreEqual(Manager.GetCountInferiors(2), 2);
            Assert.AreEqual(Manager.GetCountInferiors(-1), 2);
        }

        [TestMethod]
        public void AddPersonSample5()
        {
            Company com = CreaterCompany();
            Personnel Manager = com.Personnels.FindTo(x => x.Id == 5);

            Assert.AreEqual(Manager.Inferiors.Count, 2);

            Assert.AreEqual(Manager.GetCountInferiors(1), 2);
            Assert.AreEqual(Manager.GetCountInferiors(2), 2);
            Assert.AreEqual(Manager.GetCountInferiors(-1), 2);
        }

        [TestMethod]
        public void AddPersonSample6()
        {
            Company com = CreaterCompany();
            Personnel Manager = com.Personnels.FindTo(x => x.Id == 6);

            Assert.AreEqual(Manager.Inferiors.Count, 3);

            Assert.AreEqual(Manager.GetCountInferiors(1), 3);
            Assert.AreEqual(Manager.GetCountInferiors(2), 3);
            Assert.AreEqual(Manager.GetCountInferiors(-1), 3);
        }

        [TestMethod]
        public void AddPersonSample7()
        {
            Company com = CreaterCompany();
            Personnel Manager = com.Personnels.FindTo(x => x.Id == 7);

            Assert.AreEqual(Manager.Inferiors.Count, 3);

            Assert.AreEqual(Manager.GetCountInferiors(1), 3);
            Assert.AreEqual(Manager.GetCountInferiors(2), 3);
            Assert.AreEqual(Manager.GetCountInferiors(-1), 3);
        }

        [TestMethod]
        public void AddPersonSample()
        {
            Company com = CreaterCompany();
            for (int i = 8; i < com.Personnels.Count; i++)
            {
                Personnel Employee = com.Personnels[i];
                Assert.AreEqual(Employee.Inferiors.Count, 0);

                Assert.AreEqual(Employee.GetCountInferiors(1), 0);
                Assert.AreEqual(Employee.GetCountInferiors(2), 0);
                Assert.AreEqual(Employee.GetCountInferiors(-1), 0);
            }
        }

        [TestMethod]
        public void Edit_Name()
        {
            Company com = CreaterCompany();
            com.Edit(1, name: "NEWNAME");
            Personnel per = com.Personnels.FindTo(x => x.Id == 1);
            Assert.AreEqual(per.Name, "NEWNAME");

        }

        [TestMethod]
        public void Edit_Post()
        {
            Company com = CreaterCompany();
            com.Edit(1, post: "NEWPOST");
            Personnel per = com.Personnels.FindTo(x => x.Id == 1);
            Assert.AreEqual(per.Post, "NEWPOST");
        }

        [TestMethod]
        public void Edit_DateEmployment()
        {
            Company com = CreaterCompany();
            com.Edit(1, dateEmployment: new DateTime(2000, 12, 12));
            Personnel per = com.Personnels.FindTo(x => x.Id == 1);
            Assert.IsTrue(per.DateEmployment.Year == 2000);
            Assert.IsTrue(per.DateEmployment.Month == 12);
            Assert.IsTrue(per.DateEmployment.Day == 12);
        }

        [TestMethod]
        public void Edit_BasicRate()
        {
            Company com = CreaterCompany();
            com.Edit(1, basicRate: 1234.56);
            Personnel per = com.Personnels.FindTo(x => x.Id == 1);
            Assert.AreEqual(per.BasicRate, 1234.56);
        }

        [TestMethod]
        public void Edit_Type_Up()
        {
            Company com = CreaterCompany();
            com.Edit(8, type: PersonnelType.Manager);
            Personnel per = com.Personnels.FindTo(x => x.Id == 8);
            Assert.AreEqual(per.Type, PersonnelType.Manager);

            com.Edit(3, type: PersonnelType.Sales);
            per = com.Personnels.FindTo(x => x.Id == 3);
            Assert.AreEqual(per.Type, PersonnelType.Sales);
        }

        [TestMethod]
        public void Edit_Type_Down()
        {
            Company com = CreaterCompany();
            com.Edit(2, type: PersonnelType.Manager);
            Personnel per = com.Personnels.FindTo(x => x.Id == 2);
            Assert.AreEqual(per.Type, PersonnelType.Manager);

            Assert.ThrowsException<ExceptionLogic>(() =>
            {
                com.Edit(7, type: PersonnelType.Employee);
                per = com.Personnels.FindTo(x => x.Id == 7);
            });
        }

        [TestMethod]
        public void Edit_Chief_Manager()
        {
            Company com = CreaterCompany();
            com.Edit(3, idChief: 2);
            Personnel per = com.Personnels.FindTo(x => x.Id == 3);
            Personnel chief = com.Personnels.FindTo(x => x.Id == 2);
            Assert.AreEqual(per.Chief, chief);
            Assert.AreEqual(chief.Inferiors.Count, 3);

            Personnel old = com.Personnels.FindTo(x => x.Id == 1);
            Assert.AreEqual(old.Inferiors.Count, 1);
        }

        [TestMethod]
        public void Edit_Chief_Employee()
        {// Попытка назначит шефом работника
            Company com = CreaterCompany();
            Assert.ThrowsException<ExceptionLogic>(() =>
            {
                com.Edit(10, idChief: 8);
            });

        }

    }
}
