using Ninject;
using Owin;

namespace TestZen.Owin.WebApi
{
    public abstract class WebApiStartup : IWebApiStartup
    {
        protected WebApiStartup()
        {
            var ninjectContainer = new NinjectContainer();
            Container = ninjectContainer.GetKernel();
        }
        public IKernel Container { get; }

        public abstract void Configuration(IAppBuilder app);
    }

   
}