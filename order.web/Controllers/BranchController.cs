using order.data.contract;
using order.model;
using order.web.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
namespace order.web.Controllers
{
    [Authorize(Roles=RoleNames.CUSTOMER)]
    public class BranchController : Controller
    {
        private IOrderUow orderUow;

        public BranchController(IOrderUow uow)
        {
            orderUow = uow;
        }

        public ActionResult Index()
        {
            UserCustomerMapping usrCsMapping = orderUow.UserCustomerMapping.Where(ucm => ucm.Username == this.User.Identity.Name).FirstOrDefault();
            Customer customer = orderUow.Customers.GetById(usrCsMapping.CustomerId);
            this.ViewBag.Title = customer.CustomerName + " - " + customer.CustomerCode;
            this.Session["Title"] = customer.CustomerName + " - " + customer.CustomerCode;
            this.Session["Customer"] = customer;

            IEnumerable<Branch> allBranches = orderUow.Branches.Where(b => b.CustomerId == customer.Id).OrderByDescending(b => b.Id);
            
            return View(allBranches);
        }

        public ActionResult NewBranch()
        {
            this.ViewBag.Title = this.Session["Title"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewBranch(NewBranchViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    Roles.AddUserToRole(model.UserName, RoleNames.BRANCH);
                    Branch branch = new Branch
                    {
                        CustomerId = ((Customer)Session["Customer"]).Id,
                        BranchCode = model.BranchCode,
                        BranchName = model.BranchName,
                        Username = model.UserName
                    };
                    orderUow.Branches.Add(branch);
                    orderUow.Commit();

                    return RedirectToAction("Index", "Branch");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", AccountController.ErrorCodeToString(e.StatusCode));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.GetInnerMostException());
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult EditBranch(int id)
        {
            Branch branch = orderUow.Branches.GetById(id);
            EditBranchViewModel editBranchViewModel = new EditBranchViewModel { 
                BranchCode = branch.BranchCode,
                BranchName = branch.BranchName,
                UserName = branch.Username,
                Id = branch.Id
            };
            
            return View(editBranchViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBranch(EditBranchViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    Branch branch = orderUow.Branches.GetById(model.Id);
                    if(branch == null)
                        throw new ApplicationException("Branch " + model.BranchName + " not found");

                    WebSecurity.ChangePassword(branch.Username, model.Password, model.Password);
                    branch.BranchCode = model.BranchCode;
                    branch.BranchName = model.BranchName;
                    orderUow.Branches.Update(branch);
                    orderUow.Commit();

                    return RedirectToAction("Index", "Branch");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", AccountController.ErrorCodeToString(e.StatusCode));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.GetInnerMostException());
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult DeleteBranch(int id)
        {
            Branch branch = orderUow.Branches.GetById(id);
            return View(branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteBranch(int id)
        {
            Branch branch = orderUow.Branches.GetById(id);
            if (branch == null)
                return RedirectToAction("Index", "Branch");

            try
            {
                orderUow.Branches.Delete(branch);
                orderUow.Commit();
                Roles.RemoveUserFromRole(branch.Username, RoleNames.BRANCH);
                
                using (UsersContext db = new UsersContext())
                {
                    UserProfile userProfile = db.UserProfiles.Where(u => u.UserName == branch.Username).FirstOrDefault();
                    if (userProfile != null)
                    {
                        db.Database.ExecuteSqlCommand("delete from UserProfile where UserName = {0}", userProfile.UserName);
                        db.Database.ExecuteSqlCommand("delete from webpages_Membership where UserId = {0}", userProfile.UserId);
                    }
                }
                
                return RedirectToAction("Index", "Branch");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetInnerMostException().Message);
            }
            
            return View("~/Views/Branch/DeleteBranch.cshtml", branch);
        }
    }
}