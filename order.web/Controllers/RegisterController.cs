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
    public class RegisterController : Controller
    {
        private order.data.contract.IOrderUow Uow { get; set; }
        public RegisterController(IOrderUow uow)
        {
            Uow = uow;
        }
        public ActionResult Register()
        {
            return View();
        }

        public JsonResult ValidateLicenseId(string id)
        {
            try
            {
                Customer customer = Uow.Customers.Where(cs => cs.LicenseId == id).FirstOrDefault();
                if (customer == null)
                    throw new ApplicationException("Invalid license id");
                if (customer != null && customer.Registered)
                    throw new ApplicationException("Already registered");

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.GetInnerMostException().Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Register(SubmitLicenseIdViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Customer customer = Uow.Customers.Where( cs => cs.LicenseId == model.LicenseId).FirstOrDefault();
                    if (customer == null)
                        throw new ApplicationException("Invalid license id");
                    if (customer.Registered)
                        throw new ApplicationException("Already registered");
                    this.Session["LicenseId"] = model.LicenseId;
                    return RedirectToAction("ConfirmRegistration", "Register");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.GetInnerMostException().Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmRegistration(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    string licenseId = Request.Form["LicenseId"];
                    Customer customer = Uow.Customers.Where(cs => cs.LicenseId == licenseId).FirstOrDefault();
                    if (customer == null)
                        throw new ApplicationException("Invalid license id");
                    if (customer.Registered)
                        throw new ApplicationException("Already registered");

                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    Roles.AddUserToRole(model.UserName, RoleNames.CUSTOMER);

                    Uow.UserCustomerMapping.Add(new UserCustomerMapping { CustomerId = customer.Id, Username = model.UserName });
                    customer.Registered = true;
                    Uow.Customers.Update(customer);
                    Uow.Commit();

                    return RedirectToAction("Index", "Branch");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.GetInnerMostException().Message);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ConfirmRegistration()
        {
            return View();
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}