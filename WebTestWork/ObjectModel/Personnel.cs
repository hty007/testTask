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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WebTestWork.Models
{
    public enum PersonnelType
    {
        Employee, Manager, Sales
    }

    [Serializable]
    public class Personnel
    {
        public static PersonnelBuilder CreateBuilder()
        {
            return new PersonnelBuilder();
        }
        
        #region Serialised
        private int id;
        private string name;
        private DateTime dateEmployment;
        private double basicRate;
        private int idChief;
        //private List<int> IdInferiors;
        private string post;
        private PersonnelType _type;

        #endregion
        public Personnel()
        {
            IdChief = -1;
            BasicRate = 1000;
            //IdInferiors = new List<int>();
            Inferiors = new ObservableCollection<Personnel>();
        }
        public Personnel(PersonnelType type) : this()
        {
            _type = type;
        }

        /// <summary>Вид сотрудника </summary>
        public PersonnelType Type { get => _type; set => _type = value; }
        /// <summary>Идентификатор </summary>
        public int Id { get => id; set => id = value; }
        /// <summary>ФИО сорудника</summary>
        public string Name { get => name; set => name = value; }
        /// <summary>Должность</summary>
        public string Post { get => post; set => post = value; }
        /// <summary>Дата трудоустройства</summary>
        public DateTime DateEmployment { get => dateEmployment; set => dateEmployment = value; }
        /// <summary>Базовая ставка</summary>
        public double BasicRate { get => basicRate; set => basicRate = value; }
        /// <summary>Непосредственный начальник, null - начальников не имеет</summary>
        public Personnel Chief { get; set; }
        /// <summary>Подчиненные, null - не имеет</summary>
        public ObservableCollection<Personnel> Inferiors { get; set; }
        /// <summary>Id начальника</summary>
        public int IdChief  
        {
            get => idChief;
            set { idChief = value;}
        }
        /// <summary>
        /// Вычислить зарплату на момент указаной даты
        /// </summary>
        /// <param name="date">Дата в момент которой необходимо вычислить зарплату</param>
        /// <returns>зарплата в условных единицах</returns>
        public double GetPayment(DateTime date)
        {
            if (date < DateEmployment) return 0; // зарплата работника до его трудоустройства
            TimeSpan timeWork = date - DateEmployment; // Время работы

            if (Type == PersonnelType.Employee)
            {
                //Зарплата сотрудника Employee - это базовая ставка плюс 3 % за каждый год работы в компании, 
                //но не больше 30 % суммарной надбавки.
                double delta = Math.Round(timeWork.TotalDays / 365) * 0.03;
                if (delta > 0.3) delta = 0.3;
                return BasicRate * (1 + delta);
            }
            else if (Type == PersonnelType.Manager)
            {
                //Зарплата сотрудника Manager - это базовая ставка плюс 5 % за каждый год работы в компании
                //(но не больше 40 % суммарной надбавки за стаж работы) 
                //плюс 0,5 % зарплаты всех подчинённых первого уровня.
                double delta = Math.Round(timeWork.TotalDays / 365) * 0.05;
                if (delta > 0.4) delta = 0.4;
                double delta2 = 0;
                foreach (var inf in Inferiors)
                {
                    delta2 += inf.GetPayment(date);
                }
                return BasicRate * (1 + delta) + delta2 * 0.005;
            }
            else if (Type == PersonnelType.Sales)
            {
                //Зарплата сотрудника Sales - это базовая ставка плюс 1 % за каждый год работы в компании
                //(но не больше 35 % суммарной надбавки за стаж работы) плюс 0,3 % зарплаты всех подчинённых всех уровней.
                double delta = Math.Round(timeWork.TotalDays / 365) * 0.01;
                if (delta > 0.35) delta = 0.35;
                double delta2 = 0;
                foreach (var inf in GetAllInferiors())
                {
                    delta2 += inf.GetPayment(date);
                }
                return BasicRate * (1 + delta) + delta2 * 0.003;
            }
            return 0;
            
        }
        /// <summary>
        /// Вернуть подчиненых всех уровней (рекурсия)
        /// </summary>
        /// <returns>Лист подчиненых</returns>
        private List<Personnel> GetAllInferiors()
        {
            List<Personnel> result = new List<Personnel>();
            foreach (Personnel inf in Inferiors)
            {
                result.AddRange(inf.GetAllInferiors());
                result.Add(inf);
            }
            return result;
        }

        /// <summary>
        /// Получить количество подчиненых (рекурсия), врятли вложенность будет очень большая чтобы вызвать переполнение стека вызовов
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int GetCountInferiors(int order)
        {
            if (order == 1)
            {
                return Inferiors.Count;
            }
            else
            {
                int result = Inferiors.Count;
                foreach (var inf in Inferiors)
                {
                    result += inf.GetCountInferiors(order - 1);
                }
                return result;
            }
        }
        /// <summary>Для дебага, можно убрать</summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Id}:{Name}";
        }

        /// <summary>
        /// Проверят актуальность подчиненых, удаляет которые подчиняются другим
        /// нахордит и подписывается на шефа, добавляется в список его подчиненых
        /// </summary>
        /// <param name="company">Компания с сотрудниками</param>
        internal void Refrech(Company company)
        {
            List<Personnel> deletList = Inferiors.Where(inf => inf.IdChief != Id).ToList();
            foreach (Personnel inf in deletList)
            {
                Inferiors.Remove(inf);
            }

            if (IdChief < 1) return;
            if (IdChief == Id) IdChief = 0;// Циклическая ссылка на самого себя
            Chief = company.Personnels.FindTo(x => x.Id == IdChief);
            if (Chief != null)
            {
                if (!Chief.Inferiors.Contains(this))
                    Chief.Inferiors.Add(this);
            }

        }
    }

    public class PersonnelBuilder
    {
        private Personnel personnel;
        public PersonnelBuilder() => personnel = new Personnel();
        public PersonnelBuilder SetName(string name) { personnel.Name = name; return this; }
        public PersonnelBuilder SetId(int id) { personnel.Id = id; return this; }
        public PersonnelBuilder SetIdChief(int value) { personnel.IdChief = value; return this; }
        public PersonnelBuilder SetPost(string value) { personnel.Post = value; return this; }
        public PersonnelBuilder SetType(PersonnelType value) { personnel.Type = value; return this; }
        public PersonnelBuilder SetBasicRate(double value) { personnel.BasicRate = value; return this; }
        public PersonnelBuilder SetDateEmployment(double value) { personnel.DateEmployment = value; return this; }
        public PersonnelBuilder AddInferior(Personnel value) { personnel.Inferiors.Add(value); return this; }
        public PersonnelBuilder SetBasicRate(Personnel value) { personnel.Chief=value; return this; }
    }
}



