using Auto.Models;
using IoC;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
namespace Auto
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<ApplicationDbContext>(new AppDbInitializer());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Config Статический класс со статическим полем для хранения connectionstrng
            // Module в IoC получит доступ к этой строке
            Config.GetConnectionString("AutoContext");
            NinjectModule registrations = new Module();
            var kernel = new StandardKernel(registrations);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
