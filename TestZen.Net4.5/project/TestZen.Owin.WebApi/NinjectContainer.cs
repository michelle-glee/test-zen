using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Modules;

namespace TestZen.Owin.WebApi
{
    public class NinjectContainer
    {
        private readonly IKernel _kernel;

        public NinjectContainer()
        {
            _kernel = new StandardKernel();
        }

        public IEnumerable<INinjectModule> GetLoadedModules()
        {
            return _kernel.GetModules();
        }

        public IKernel GetKernel()
        {
            return _kernel;
        }

        public void Load(params INinjectModule[] modules)
        {
            _kernel.Load(modules);
        }

        public T Resolve<T>()
        {
            return _kernel.Get<T>();
        }

        public T TryResolve<T>(Type type) where T : class
        {
            return _kernel.TryGet(type) as T;
        }
    }
}