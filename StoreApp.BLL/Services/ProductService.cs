using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.BLL.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork database;
        private IMapper config;

        public ProductService(IUnitOfWork uof)
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
            }).CreateMapper();
            database = uof;
        }

        public ProductService()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
            }).CreateMapper();
            database = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(ProductDTO product)
        {
            var productDAL = config.Map<ProductDTO, Product>(product);
            database.Products.Create(productDAL);
            database.Save();
        }

        public void Delete(int id)
        {
            database.Products.Delete(id);
            database.Save();
        }

        public void Edit(ProductDTO product)
        {
            var productDAL = config.Map<ProductDTO, Product>(product);
            database.Products.Update(productDAL);
            database.Save();
        }

        public ProductDTO Get(int id)
        {
            return config.Map<Product, ProductDTO>(database.Products.Get(id));
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            return config.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(database.Products.GetAll());
        }

        public bool ValidateNewProduct(ProductDTO product)
        {
            return !database.Products.Find(p => p.Name.Equals(product.Name) &&
                                                p.BrandId.Equals(product.BrandId) &&
                                                p.ProducerId.Equals(product.ProducerId)).Any();
        }

        public bool ValidateEditProduct(ProductDTO productDTO)
        {
            var products = database.Products.GetAll();

            products.ToList().ForEach(p => database.Products.Detach(p));

            return !products.Any(p => p.Id != productDTO.Id &&
                                      p.Name.Equals(productDTO.Name) &&
                                      p.BrandId.Equals(productDTO.BrandId) &&
                                      p.ProducerId.Equals(productDTO.ProducerId));
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
