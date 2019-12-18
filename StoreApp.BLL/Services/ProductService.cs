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
        private IUnitOfWork DataBase { get; set; }

        private IMapper config;

        public ProductService(IUnitOfWork uof)
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
            }).CreateMapper();
            DataBase = uof;
        }

        public ProductService()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<ProductDTO, Product>();
            }).CreateMapper();
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

        public bool ValidateNewProduct(ProductDTO product)
        {
            var validate = DataBase.Products.Find(p => p.Name.Equals(product.Name) &&
                                                       p.BrandId.Equals(product.BrandId) &&
                                                       p.ProducerId.Equals(product.ProducerId));

            foreach (var item in validate)
            {
                return false;
            }

            return true;
        }

        public bool ValidateEditProduct(ProductDTO productDTO)
        {
            Product product = DataBase.Products.Get(productDTO.Id);

            if (product.Name == productDTO.Name &&
                product.BrandId == productDTO.BrandId &&
                product.ProducerId == productDTO.ProducerId)
            {
                return true;
            }

            var validate = DataBase.Products.GetAll()
                                            .Any(p => p.Name.Equals(productDTO.Name) &&
                                                      p.BrandId.Equals(productDTO.BrandId) &&
                                                      p.ProducerId.Equals(productDTO.ProducerId));

            if (validate)
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
