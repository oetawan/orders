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
using order.snapshot;
using StructureMap;
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
            ViewBag.VendorName = OrderSession.Vendor.Name;
            
            return View();
        }

        [HttpGet]
        public JsonResult Groups()
        {
            IList<Grouping> groups = new List<Grouping>();
            using (var ch = OrderSession.OrderServiceChannelFactory.CreateChannel())
            {
                groups = ch.AllGroups();
            }
            
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ItemsGroup(int groupId)
        {
            IList<Item> items = new List<Item>();
            using (var ch = OrderSession.OrderServiceChannelFactory.CreateChannel())
            {
                items = ch.FindItemByGroup(groupId);
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult SearchItem(string searchQuery)
        {
            IList<Item> items = new List<Item>();
            using (var ch = OrderSession.OrderServiceChannelFactory.CreateChannel())
            {
                items = ch.SearchItem(searchQuery);
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ShoppingCart()
        {
            ShoppingCart sc = orderUow.ShoppingCarts.Get(this.User.Identity.Name);
            return Json(sc.CreateSnapshot(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddItemToOrder(ShoppingCart.AddItemCommand cmd)
        {
            return CatchPosibleExeption(() =>
            {
                cmd.Username = this.User.Identity.Name;
                order.service.contract.IOrderService orderService = ObjectFactory.GetInstance<order.service.contract.IOrderService>();
                orderService.AddItemToOrder(cmd);

                return Json(new { success = true });
            });
        }

        [HttpPost]
        public JsonResult RemoveItem(ShoppingCart.RemoveItemCommand cmd)
        {
            return CatchPosibleExeption(() =>
            {
                cmd.Username = this.User.Identity.Name;
                order.service.contract.IOrderService orderService = ObjectFactory.GetInstance<order.service.contract.IOrderService>();
                orderService.RemoveItem(cmd);
                
                return Json(new { success = true });
            });
        }

        private JsonResult CatchPosibleExeption(Func<JsonResult> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.GetInnerMostException().Message });
            }
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