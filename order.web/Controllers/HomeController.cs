using order.data.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace order.web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private order.data.contract.IOrderUow Uow { get; set; }
        public HomeController(IOrderUow uow)
        {
            Uow = uow;
        }
        public ActionResult Index()
        {
            ViewBag.Message = "Customers";

            return View(Uow.Customers.GetAll());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
