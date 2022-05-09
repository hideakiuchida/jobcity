using Microsoft.AspNet.SignalR.Hubs;
using Unity;

namespace Jobcity.Chat.IoC
{
    public class UnityHubActivator : IHubActivator
    {

        private readonly IUnityContainer _container;

        public UnityHubActivator(IUnityContainer container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)_container.Resolve(descriptor.HubType);
        }
    }
}