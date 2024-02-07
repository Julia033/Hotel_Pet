using Hotel_Pet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel_Pet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult ListOfCare()
        {
            List<Care> cares = new List<Care>();
            using (var db = new Nazarenko_IDZEntities())
            {
                cares = db.Cares.OrderByDescending(x => x.Price)
                    .ThenBy(x => x.Type).ToList();
            }
            return View(cares);
        }
        
    }
}