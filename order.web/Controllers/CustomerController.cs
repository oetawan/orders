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
using WebMatrix.WebData;

namespace order.web.Controllers
{
    [Authorize(Roles=RoleNames.ADMINISTRATOR)]
    public class CustomerController : Controller
    {
        private order.data.contract.IOrderUow Uow { get; set; }
        public CustomerController(IOrderUow uow)
        {
            Uow = uow;
        }
        public ActionResult Index()
        {
            return View(Uow.Customers.GetAll().OrderByDescending<Customer,int>(cs => cs.Id));
        }

        public ActionResult NewCustomer()
        {
            return View();
        }

        public ActionResult EditCustomer(int id)
        {
            Customer customer = Uow.Customers.GetById(id);
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCustomer(Customer model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Uow.Customers.Add(model);
                    Uow.Commit();
                    return RedirectToAction("Index", "Customer");
                }
                catch (DbUpdateException ex)
                {
                    string errorMessage = ex.GetInnerMostException().Message;
                    if(errorMessage.Contains("duplicate key") && errorMessage.Contains("LicenseId"))
                        ModelState.AddModelError(typeof(DbUpdateException).Name, string.Format("License Id {0} already taken", model.LicenseId));
                    else if(errorMessage.Contains("duplicate key") && errorMessage.Contains("CustomerCode"))
                        ModelState.AddModelError(typeof(DbUpdateException).Name, string.Format("Customer Code {0} already taken", model.CustomerCode));
                    else
                        ModelState.AddModelError(typeof(DbUpdateException).Name, errorMessage);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(Customer model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Customer customer = Uow.Customers.GetById(model.Id);
                    if (customer != null)
                    {
                        customer.CustomerCode = model.CustomerCode;
                        customer.CustomerName = model.CustomerName;
                        customer.Issuer = model.Issuer;
                        customer.LicenseId = model.LicenseId;
                        customer.SecretKey = model.SecretKey;
                        customer.ServiceBusNamespace = model.ServiceBusNamespace;
                        
                        Uow.Customers.Update(customer);
                        Uow.Commit();
                    }
                    
                    return RedirectToAction("Index", "Customer");
                }
                catch (DbUpdateException ex)
                {
                    string errorMessage = ex.GetInnerMostException().Message;
                    if (errorMessage.Contains("duplicate key") && errorMessage.Contains("LicenseId"))
                        ModelState.AddModelError(typeof(DbUpdateException).Name, string.Format("LicenseId {0} already taken", model.LicenseId));
                    else if (errorMessage.Contains("duplicate key") && errorMessage.Contains("CustomerCode"))
                        ModelState.AddModelError(typeof(DbUpdateException).Name, string.Format("Customer Code {0} already taken", model.CustomerCode));
                    else
                        ModelState.AddModelError(typeof(DbUpdateException).Name, errorMessage);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e);
                }
            }

            return View(model);
        }

        public ActionResult DeleteCustomer(int id)
        {
            Customer customer= Uow.Customers.GetById(id);
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteCustomer(int id)
        {
            Customer model = Uow.Customers.GetById(id);
            if (model == null)
                return RedirectToAction("Index", "Customer");

            try
            {
                Uow.Customers.Delete(model);
                Uow.Commit();
                return RedirectToAction("Index", "Customer");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e);
            }
            
            return View(model);
        }

        [AllowAnonymous]
        public JsonResult Login(CustomerLoginModel login)
        {
            try
            {
                if (WebSecurity.Login(login.Username, login.Password))
                {
                    if (!Roles.IsUserInRole(login.Username, RoleNames.CUSTOMER))
                        throw new ApplicationException("You are not authorized");
                    
                    UserCustomerMapping ucm = Uow.UserCustomerMapping.Where(u => u.Username == login.Username).FirstOrDefault();

                    if (ucm == null)
                        throw new ApplicationException("You are not registered");

                    Customer customer = Uow.Customers.GetById(ucm.CustomerId);

                    if (customer == null)
                        throw new ApplicationException("You are not registered");

                    EndpointConfig endpointConfig = new EndpointConfig
                    {
                        ServiceBusNamespace = customer.ServiceBusNamespace,
                        Issuer = customer.Issuer,
                        SecretKey = customer.SecretKey,
                        Error = false
                    };

                    return Json(endpointConfig);
                }
                else
                {
                    throw new ApplicationException("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                return Json(new EndpointConfig { Error = true, ErrorMessage = ex.GetInnerMostException().Message });
            }
            finally
            {
                WebSecurity.Logout();
            }
        }
    }
}