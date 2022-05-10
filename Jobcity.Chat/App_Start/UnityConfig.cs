using Jobcity.Chat.ApplicationLayer.Contracts;
using Jobcity.Chat.ApplicationLayer.Implementations;
using Jobcity.Chat.Bot.Contracts;
using Jobcity.Chat.Bot.Implementations;
using Jobcity.Chat.InfraLayer.Contracts;
using Jobcity.Chat.InfraLayer.Implementations;
using Jobcity.Chat.IoC;
using Jobcity.Chat.Mvc.Controllers;
using Jobcity.Chat.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Data.Entity;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;

namespace Jobcity.Chat.Mvc
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<IChatMessageRepository, ChatMessageRepository>();
            container.RegisterType<IChatApiProxy, ChatApiProxy>();
            container.RegisterType<IChatBotBL, ChatBotBL>();
            container.RegisterType<IChatBotRabbitProxy, ChatBotRabbitProxy>();
            container.RegisterType<IChatMessageBL, ChatMessageBL>();
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new UnityHubActivator(container));
        }
    }
}