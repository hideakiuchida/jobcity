using Jobcity.Chat.ApplicationLayer.Contracts;
using Jobcity.Chat.ApplicationLayer.Implementations;
using Jobcity.Chat.InfraLayer.Contracts;
using Jobcity.Chat.InfraLayer.Implementations;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Jobcity.Chat.Api
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
            container.RegisterType<IChatBotBL, ChatBotBL>();
            container.RegisterType<IChatBotProxy, ChatBotProxy>();
        }
    }
}