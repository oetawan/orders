using order.data.contract;
using order.model;
using order.web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace order.web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string[] roles = Roles.GetRolesForUser(this.User.Identity.Name);

            if (roles.Contains(RoleNames.ADMINISTRATOR))
                return RedirectToAction("Index", "Customer");
            else if (roles.Contains(RoleNames.CUSTOMER))
                return RedirectToAction("Index", "Branch");
            else if (roles.Contains(RoleNames.BRANCH))
                return RedirectToAction("Index", "Order");

            return RedirectToAction("Login", "Account");
        }
    }
}