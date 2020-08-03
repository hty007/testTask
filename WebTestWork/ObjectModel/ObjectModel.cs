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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebTestWork.Models        
{
    public class DataService
    { 
        
    }

    /// <summary>
    /// Объектная модель, все данные хранятся здесь
    /// </summary>
    public static class ObjectModel
    {
        /// <summary>
        /// Компания в которой находятся сотрудники
        /// </summary>
        public static Company Company;

        static ObjectModel()
        {
            FileInfo data = new FileInfo("data.bin");
            DataPath = data.FullName;
            if (data.Exists)
            {
                LoadData();
            }
            else
            {
                Company = new Company();
                SaveData();
            }
        }

        private static readonly string DataPath;
        private static void LoadData()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                // десериализация из файла people.dat
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate))
                {
                    Company = (Company)formatter.Deserialize(fs);
                }
                Company.Refrech();

            }
            catch (Exception ex)
            {
                if (Company == null) Company = new Company();
            }
        }
        public static void SaveData()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Company);
                Console.WriteLine("Объект сериализован");
            }
        }

    }


}

