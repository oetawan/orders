using Microsoft.ServiceBus;
using order.data.contract;
using order.model;
using porder.model;
using porder.service.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Web;
namespace order.web.Models
{
    public class OrderSession
    {
        const string BRANCH = "Branch";
        const string CUSTOMER = "Customer";
        const string VENDOR = "Vendor";
        const string ORDER_SERVICE_CHANNEL_FACTORY = "OrderServiceChannelFactory";

        public Branch Branch { 
            get {
                if (httpSession[BRANCH] == null)
                {
                    httpSession[BRANCH] = orderUow.Branches.Where(b => b.Username == this.user.Identity.Name).FirstOrDefault();
                }

                return (Branch)httpSession[BRANCH];
            }
        }

        public Customer Customer{
            get {
                if (httpSession[CUSTOMER] == null)
                {
                    httpSession[CUSTOMER] = orderUow.Customers.GetById(Branch.CustomerId);
                }

                return (Customer)httpSession[CUSTOMER];
            }
        }

        public Vendor Vendor
        {
            get
            {
                if (httpSession[VENDOR] == null)
                {
                    using(var ch = OrderServiceChannelFactory.CreateChannel())
                    {
                        httpSession[VENDOR] = ch.FindVendorByCode(Branch.BranchCode);
                    }
                }

                return (Vendor)httpSession[VENDOR];
            }
        }

        public ChannelFactory<IOrderServiceChannel> OrderServiceChannelFactory {
            get {
                if (httpSession[ORDER_SERVICE_CHANNEL_FACTORY] == null)
                {
                    var cf = new ChannelFactory<IOrderServiceChannel>(
                        new NetTcpRelayBinding(),
                        new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb", Customer.ServiceBusNamespace, "order")));
                    
                    cf.Endpoint.EndpointBehaviors.Add(new TransportClientEndpointBehavior
                    {
                        TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(
                            Customer.Issuer, 
                            Customer.SecretKey)
                    });

                    httpSession[ORDER_SERVICE_CHANNEL_FACTORY] = cf;
                }

                return (ChannelFactory<IOrderServiceChannel>)httpSession[ORDER_SERVICE_CHANNEL_FACTORY];
            }
        }

        public OrderSession(IOrderUow uow, HttpSessionStateBase httpSession, IPrincipal usr)
        {
            this.orderUow = uow;
            this.httpSession = httpSession;
            this.user = usr;
        }

        private IOrderUow orderUow;
        private HttpSessionStateBase httpSession;
        private System.Security.Principal.IPrincipal user;
    }
}