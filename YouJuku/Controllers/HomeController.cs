using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YouJuku.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Scheduler");
            //if(User.IsInRole("Admin")) 
            // {
            //     return RedirectToAction("Index", "SchedulerAdmin");
            // }
            // else
            // {
            //     return RedirectToAction("Index", "Scheduler");
            // }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}