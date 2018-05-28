using System;
using Ninject.Modules;
using Ninject.Web.Common;

namespace TestZen.WebApi.Example
{
    public class MyNinjectModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IMyInterface>().To<MyClass>().InRequestScope();
        }
    }
}
