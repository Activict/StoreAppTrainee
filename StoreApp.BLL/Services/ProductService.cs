using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    class ProductService : IProductService
    {
        IUnitOfWork DataBase;
        public ProductService(IUnitOfWork uof)
        {
            DataBase = uof;
        }
        public void Create(ProductDTO product)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()).CreateMapper();
            Product productDAL = config.Map<ProductDTO, Product>(product);
            DataBase.Products.Create(productDAL);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.Products.Delete(id);
            DataBase.Save();
        }

        public void Edit(ProductDTO product)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()).CreateMapper();
            Product productDTO = config.Map<ProductDTO, Product>(product);
            DataBase.Products.Update(productDTO);
            DataBase.Save();
        }

        public ProductDTO Get(int id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, Product>()).CreateMapper();
            return config.Map<Product, ProductDTO>(DataBase.Products.Get(id));
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()).CreateMapper();
            return config.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(DataBase.Products.GetAll());
        }
        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
