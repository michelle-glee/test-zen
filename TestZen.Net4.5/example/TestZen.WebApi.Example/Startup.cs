using System.ComponentModel;
using System.Web.Http;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using TestZen.WebApi.Example;
using TestZen.Owin.WebApi;


[assembly: OwinStartup(typeof(Startup))]
namespace TestZen.WebApi.Example
{

    public class Startup : WebApiStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            //Load your Ninject Modules for IoC
            Container.Load(new MyNinjectModule());

            //Do your normal Startup stuff 
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            //Initialise the Ninject middleware
            app.UseNinjectMiddleware(() => Container);

            //Register the WebApi through Ninject
            app.UseNinjectWebApi(config);

      }
    }
}
