using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork DataBase { get; set; }

        private IMapper config;

        public ProductService(IUnitOfWork uof)
        {
            config = new MapperConfiguration(cfg => { cfg.CreateMap<Product, ProductDTO>();
                                                      cfg.CreateMap<ProductDTO, Product>(); }).CreateMapper();
            DataBase = uof;
        }

        public ProductService()
        {
            config = new MapperConfiguration(cfg => { cfg.CreateMap<Product, ProductDTO>();
                                                      cfg.CreateMap<ProductDTO, Product>(); }).CreateMapper();
            DataBase = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(ProductDTO product)
        {
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
            Product productDAL = config.Map<ProductDTO, Product>(product);
            DataBase.Products.Update(productDAL);
            DataBase.Save();
        }

        public ProductDTO Get(int id)
        {
            return config.Map<Product, ProductDTO>(DataBase.Products.Get(id));
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            return config.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(DataBase.Products.GetAll());
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
