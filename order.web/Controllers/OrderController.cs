using order.data.contract;
using order.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using porder.service.contract;
using porder.model;
using Microsoft.ServiceBus;
using order.web.Models;
namespace order.web.Controllers
{
    public class OrderController : Controller
    {
        const string ORDER_SESSION = "OrderSession";
        IOrderUow orderUow;
        
        public OrderController(IOrderUow uow)
        {
            this.orderUow = uow;
        }

        public ActionResult Index()
        {
            ViewBag.Title = OrderSession.Branch.BranchName;

            IList<Grouping> groups = new List<Grouping>();
            using (var ch = OrderSession.OrderServiceChannelFactory.CreateChannel())
            {
                groups = groups = ch.AllGroups();
            }

            return View(groups);
        }

        private OrderSession OrderSession
        {
            get
            {
                if (Session[ORDER_SESSION] == null)
                {
                    Session[ORDER_SESSION] = new OrderSession(orderUow, this.Session, this.User);
                }

                return (OrderSession)Session[ORDER_SESSION];
            }
        }
    }
}