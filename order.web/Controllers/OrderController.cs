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
using System.Transactions;
namespace order.web.Controllers
{
    public class OrderController : Controller
    {
        const string ORDER_SESSION = "OrderSession";
        IOrderUow orderUow;
        OrderNumber orderNumber;
        porder.model.Order po;
        
        public OrderController(IOrderUow uow)
        {
            this.orderUow = uow;
        }

        public ActionResult Index()
        {
            ViewBag.Title = OrderSession.Branch.BranchName;
            ViewBag.VendorName = OrderSession.Customer.CustomerName;
            
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
        public JsonResult ChangeQty(ShoppingCart.ChangeQtyCommand cmd)
        {
            return CatchPosibleExeption(() =>
            {
                cmd.Username = this.User.Identity.Name;
                order.service.contract.IOrderService orderService = ObjectFactory.GetInstance<order.service.contract.IOrderService>();
                orderService.ChangeQty(cmd);

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

        [HttpPost]
        public JsonResult Checkout()
        {
            return CatchPosibleExeption(() =>
            {
                orderNumber = this.orderUow.ShoppingCarts.GetOrderNumber(OrderSession.Branch.BranchCode);
                orderNumber.Next();
                this.orderUow.ShoppingCarts.SaveOrderNumber(orderNumber);
                
                using (var porderServiceChannel = OrderSession.OrderServiceChannelFactory.CreateChannel())
                {
                    ShoppingCart sc = this.orderUow.ShoppingCarts.Get(this.User.Identity.Name);
                    if (sc == null)
                        throw new ApplicationException("Shopping cart not found");

                    ShoppingCartSnapshot snapshot = sc.CreateSnapshot();
                    List<porder.model.OrderItem> porderItems = new List<porder.model.OrderItem>();
                    int orderItemSequence = 0;
                    foreach (ShoppingCartItemSnapshot itemSnapshot in snapshot.Items)
                    {
                        porderItems.Add(new porder.model.OrderItem
                        {
                            SOSeq = ++orderItemSequence,
                            ItemID = itemSnapshot.ItemId,
                            UnitCode = itemSnapshot.UnitCode,
                            Price = itemSnapshot.Price,
                            Quantity = itemSnapshot.Qty,
                            GrossAmt = itemSnapshot.AmountAfterDiscount,
                            SubTotal = itemSnapshot.AmountAfterDiscount
                        });
                    }
                    po = new porder.model.Order
                    {
                        Items = porderItems,
                        BranchID = OrderSession.Branch.BranchCode,
                        SOCode = orderNumber.OrderNumberString(),
                        CurrencyId = "IDR",
                        SODate = DateTime.Today,
                        Username = this.User.Identity.Name,
                        VendorID = this.OrderSession.Branch.BranchCode,
                        SOGrossAmt = snapshot.TotalAmountAfterDiscount,
                        SONetAmt = snapshot.TotalAmountAfterDiscount
                    };
                    porder.model.CreateOrderResponse response = porderServiceChannel.CreateOrder(po);
                    if (response.Error)
                        throw new ApplicationException(response.ErrorMessage);
                };

                order.model.ShoppingCart.CheckoutCommand cmd = new ShoppingCart.CheckoutCommand
                {
                    BranchId = OrderSession.Branch.Id,
                    OrderDate = po.SODate,
                    OrderNumber = this.orderNumber.OrderNumberString(),
                    BranchCode = OrderSession.Branch.BranchCode,
                    Username = this.User.Identity.Name
                };

                var explicitArgs = new StructureMap.Pipeline.ExplicitArguments();
                explicitArgs.SetArg("orderUow", this.orderUow);

                order.service.contract.IOrderService orderService = ObjectFactory.GetInstance<order.service.contract.IOrderService>(explicitArgs);
                orderService.CheckoutOut(cmd, () => { this.orderUow.ShoppingCarts.SaveOrderNumber(orderNumber); });

                return Json(new { success = true });
            });
        }

        [HttpGet]
        public JsonResult OrderItems(int orderId)
        {
            return CatchPosibleExeption(() =>
            {
                IList<order.model.OrderItem> result = orderUow.ShoppingCarts.FindOrderItem(orderId);

                return Json(result, JsonRequestBehavior.AllowGet);
            });
        }

        [HttpGet]
        public JsonResult All()
        {
            return CatchPosibleExeption(() => {

                IList<order.model.Order> result = orderUow.Orders.
                    Where(o => o.BranchId == OrderSession.Branch.Id).
                    OrderByDescending(o => o.OrderDate).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
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