﻿using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using StoreApp.Util;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StoreApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //NinjectModule registrations = new DIRegistration();
            //var kernel = new StandardKernel(registrations);
            //kernel.Unbind<ModelValidatorProvider>();
            //DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
