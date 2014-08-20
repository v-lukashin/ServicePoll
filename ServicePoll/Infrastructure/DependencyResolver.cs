using Microsoft.Practices.ServiceLocation;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;
using ServicePoll.Models;
using ServicePoll.Repository;
using ServicePoll.Repository.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace ServicePoll.Infrastructure
{
    public class DependencyResolver : Scope, IDependencyResolver, IServiceLocator
    {
        private IKernel _kernel;
        public DependencyResolver(IKernel kernel):base(kernel)
        {
            _kernel = kernel;
            AddBindings();
        }
        private void AddBindings()
        {
            _kernel.Bind<IRep<Poll>>().To<Repository<Poll>>();
            _kernel.Bind<IRep<Result>>().To<Repository<Result>>();
        }
        public IDependencyScope BeginScope()
        {
            return new Scope(_kernel.BeginBlock());
        }
        
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return _kernel.GetAll<TService>();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        public TService GetInstance<TService>(string key)
        {
            return _kernel.TryGet<TService>(key);
        }

        public TService GetInstance<TService>()
        {
            return _kernel.TryGet<TService>();
        }

        public object GetInstance(Type serviceType, string key)
        {
            return _kernel.TryGet(serviceType, key);
        }

        public object GetInstance(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }
    }
}