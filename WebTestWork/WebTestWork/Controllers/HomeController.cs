using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebTestWork.Models;
/*
 Нужно реализовать библиотеку с бизнес-логикой, unit тесты и standalone ASP.NET Core приложение. Число проектов в решении не ограничено. 
Одна страничка, которая должна:
- дать возможность получить расчет зарплаты любого сотрудника на произвольный момент времени
- отображать зарплату всех сотрудников
Сохранять/загружать ничего не нужно.
 */


namespace WebTestWork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Company = ObjectModel.Company;
            return View();
        }

        //public IActionResult ShowNow(int id_now)
        //{            
        //    Personnel personnel = ObjectModel.Company.Personnels.FindTo(p => p.Id == id_now);
        //    return RedirectToAction("Show", "Home", new { id = id_now, date = DateTime.Now });
        //}

        [HttpGet]
        public IActionResult Show(int id)
        {
            Personnel personnel = ObjectModel.Company.Personnels.FindTo(p => p.Id == id);
            this.ViewBag.Date = DateTime.Now;
            return View(personnel);
        }

        [HttpPost]
        public IActionResult Show(int id, DateTime date)
        {
            Personnel personnel = ObjectModel.Company.Personnels.FindTo(p => p.Id == id);
            this.ViewBag.Date = date;            
            return View(personnel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddPerson()
        {
            this.ViewBag.Post = false;
            return View();
        }

        [HttpPost]
        public IActionResult AddPerson(Personnel personnel)
        {
            //this.
            ObjectModel.Company.Add(personnel);
            
            //this.ViewBag.Post = true;
            //ViewBag.Personnel = personnel;
            return Redirect("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Personnel personnel = ObjectModel.Company.Personnels.FindTo(p => p.Id == id);
            return View(personnel);
        }

        [HttpPost]
        public IActionResult Edit(Personnel personnel)
        {
            ObjectModel.Company.Edit(personnel.Id,
                name: personnel.Name,
                post: personnel.Post,
                type: personnel.Type,
                idChief:personnel.IdChief,
                basicRate: personnel.BasicRate,
                dateEmployment: personnel.DateEmployment);
            ObjectModel.SaveData();
            return Redirect("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
