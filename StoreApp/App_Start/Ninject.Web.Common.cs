[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StoreApp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(StoreApp.App_Start.NinjectWebCommon), "Stop")]

namespace StoreApp.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using StoreApp.BLL.Interfaces;
    using StoreApp.BLL.Services;
    using StoreApp.DAL.Intefaces;
    using StoreApp.DAL.Repositories;
    using StoreApp.Util;
    using System;
    using System.Web;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument("DefaultConnection");

            kernel.Bind<IWebMapper>().To<WebMapper>();

            kernel.Bind<IBrandService>().To<BrandService>();
         }        
    }
}