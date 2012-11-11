using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using order.web.Models;
using System.Web.Security;

namespace order.web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("Account", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    InitializeAdminRoleAndUser();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }

            private void InitializeAdminRoleAndUser()
            {
                if (!Roles.RoleExists(RoleNames.ADMINISTRATOR))
                    Roles.CreateRole(RoleNames.ADMINISTRATOR);

                if (!WebSecurity.UserExists(Defaults.ADMIN_USER))
                {
                    WebSecurity.CreateUserAndAccount(Defaults.ADMIN_USER, Defaults.ADMIN_PASSWORD, null, false);
                    Roles.AddUserToRole(Defaults.ADMIN_USER, RoleNames.ADMINISTRATOR);
                }
            }
        }
    }
}