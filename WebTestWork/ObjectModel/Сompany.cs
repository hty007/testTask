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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WebTestWork.Models
{
    
    /// <summary>
    /// Методы раширения коллекций
    /// </summary>
    public static class ExtensionCollectionClass
    {
        /// <summary>
        /// Найти в коллекции <paramref name="collect"/> первое вхождение удовлетворяющее <paramref name="predicate"/> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collect">Коллекция</param>
        /// <param name="predicate">Условие удовлетвояющее поиску</param>
        /// <returns>Элемент коллекции</returns>
        public static T FindTo<T>(this IEnumerable<T> collect, Predicate<T> predicate)
        {
            foreach (T item in collect)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            return default(T);
        }
        /// <summary>
        /// Добавить в коллекцию коллекцию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="THIS">В неё добовряем</param>
        /// <param name="collect">её элементы будут добавлены</param>
        /// <remarks> Позже окозалось что данный метод не нужен. Он был написан для класса <see cref="ObservableCollection"/> в нем нет похожей реализации </remarks>
        public static void AddCollect<T>(this ICollection<T> THIS, IEnumerable<T> collect)
        {
            foreach (T item in collect)
            {
                THIS.Add(item);
            }

        }


    }

    [Serializable]
    public class Company
    {
        /// <summary>
        /// Название компании
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание компании
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Весь Персонал
        /// </summary>
        public ObservableCollection<Personnel> Personnels;

        public Company()
        {
            Name = "Тестовая комания";
            Description = "Копания  по ...";
            Personnels = new ObservableCollection<Personnel>();
            //Personnels.Add(new Personnel(PersonnelType.Manager)
            //{
            //    Id = 1,
            //    Name = "Иван",
            //    Post = "Директор",
            //    DateEmployment = new DateTime(2000, 1, 1)
            //});
        }

        /// <summary>
        /// Обновить список, проверить на уникальность ID
        /// </summary>
        internal void Refrech()
        {
            List<Personnel> deleteList = new List<Personnel>();
            foreach (Personnel per in Personnels)
            {
                if (deleteList.Contains(per)) continue;
                per.Refrech(this);
                if (per.Id < 1)
                {
                    per.Id = Personnels.Max(p => p.Id) + 1;
                }
                else
                {
                    List<Personnel> collect = Personnels.Where(x => x.Id == per.Id).ToList();
                    if (collect.Count > 1)
                    {// ошибка несколько элементов с одинаковым id  удаляем лишние
                        bool uno = true;
                        foreach (var item in collect)
                        {
                            if (uno) uno = false;// подобие триггера
                            else
                            {
                                deleteList.Add(item);
                            }
                        }

                    }
                }
            }
            foreach (var item in deleteList)
            {
                Personnels.Remove(item);
            }
        }

        /// <summary>
        /// Добавить одного сотрудника в компанию
        /// </summary>
        /// <param name="personnel">новый сотрудник</param>
        /// <remarks> При добавлении ему автоматически назначается уникальный ID, и сохраняется <c> ObjectModel</c> (не обязательно, потом нужно удалить) </remarks>
        public void Add(Personnel personnel)
        {
            if (Personnels.Count == 0) 
                personnel.Id = 1;
            else 
                personnel.Id = Personnels.Max(p => p.Id) + 1; 
            Personnels.Add(personnel);
            Refrech();
            ObjectModel.SaveData();
        }

        /// <summary>
        /// Редактировать запись с id на данные data
        /// </summary>
        /// <param name="id">номер редактируемой записи</param>
        /// <param name="name">новое имя</param>
        /// <param name="post">новый пост</param>
        /// <param name="type">новый тип</param>
        /// <param name="idChief">новый шеф</param>
        /// <exception cref="ExceptionLogic">При изменении типа на Employee, в случае если у него есть подчиненые</exception>
        /// <exception cref="ExceptionLogic">При попытке назначить начальником сотрудника типом Employee</exception>
        public void Edit(int id, string name = null, string post = null, 
            PersonnelType? type = null, int idChief = -1, double basicRate = 0,
            DateTime? dateEmployment = null)
        {
            Personnel per = Personnels.FindTo(x => x.Id == id);
            if (per != null)
            {
                if (name != null) per.Name = name;
                if (post != null) per.Post = post;

                if (type != null)
                {
                    if (type == PersonnelType.Employee && per.Inferiors.Count == 0)
                        per.Type = (PersonnelType)type;
                    else if (type != PersonnelType.Employee)
                        per.Type = (PersonnelType)type;
                    else
                        throw new ExceptionLogic("L001: Попытка изменить тип сотрудника на 'PersonnelType.Employee' при наличии у него подчиненых!");
                }

                if (idChief > 0)
                {
                    Personnel newChief = Personnels.FindTo(x => x.Id == idChief);
                    if (newChief.Type == PersonnelType.Employee)
                        throw new ExceptionLogic("L002: попытка назначить начальником сотрудника типом Employee");

                    Personnel old = per.Chief;
                    per.IdChief = idChief;
                    old.Refrech(this);
                }
                if (basicRate > 0) per.BasicRate = basicRate;
                if (dateEmployment != null) per.DateEmployment = (DateTime)dateEmployment;

                per.Refrech(this);
            }
        }
    }
}



