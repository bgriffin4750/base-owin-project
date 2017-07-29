namespace BaseOwinService.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;
    using Microsoft.Practices.Unity;

    public class UnityResolver : IDependencyResolver
    {
        private bool _disposing;

        public UnityResolver(IUnityContainer container)
        {
            Container = container;
        }

        protected IUnityContainer Container { get; }

        public void Dispose()
        {
            if (_disposing) return;
            _disposing = true;

            Container?.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return Container.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = Container.CreateChildContainer();
            return new UnityResolver(child);
        }
    }
}