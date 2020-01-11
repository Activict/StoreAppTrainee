using Ninject;
using StoreApp.BLL.Interfaces;
using StoreApp.BLL.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StoreApp.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //kernel.Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument("DefaultConnection");
            kernel.Bind<IBrandService>().To<BrandService>();

        }
    }
}