using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace ServicePoll.Infrastructure
{
    public class Scope: IDependencyScope
    {
        private IResolutionRoot _resolutionRoot;
        public Scope(IResolutionRoot root)
        {
            _resolutionRoot = root;
        }
        public object GetService(Type serviceType)
        {
            IRequest request = _resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _resolutionRoot.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = _resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _resolutionRoot.Resolve(request).ToList();
        }

        public void Dispose()
        {
            IDisposable disposable = (IDisposable)_resolutionRoot;
            if (disposable != null) disposable.Dispose();
            _resolutionRoot = null;
        }
    }
}