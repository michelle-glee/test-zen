using Ninject;
using Owin;

namespace TestZen.Owin.WebApi
{
    public interface IWebApiStartup
    {
        IKernel Container { get; }
        void Configuration(IAppBuilder app);
    }
}
