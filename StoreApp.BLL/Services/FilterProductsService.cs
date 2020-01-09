using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.BLL.Services
{
    public class FilterProductsService
    {
        private IUnitOfWork database;
        private IMapper config;

        public FilterProductsService()
        {
            database = new EFUnitOfWork("DefaultConnection");
            config = new MapperConfiguration(cfg => { cfg.CreateMap<Product, ProductDTO>(); }).CreateMapper();
        }
        public FilterProductsService(IUnitOfWork uof)
        {
            database = uof;
            config = new MapperConfiguration(cfg => { cfg.CreateMap<Product, ProductDTO>(); }).CreateMapper();
        }

        public IEnumerable<ProductDTO> GetProductsDTOFiltered(FilterProductsDTO filter)
        {
            var products = config.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(database.Products.GetAll());

            if (!String.IsNullOrEmpty(filter.Name))
            {
                products = products.Where(p => p.Name.Contains(filter.Name));
            }

            if (filter.PriceFrom > 0)
            {
                products = products.Where(p => p.Price >= filter.PriceFrom);
            }

            if (filter.PriceTo > 0)
            {
                products = products.Where(p => p.Price < filter.PriceTo);
            }

            if (filter.Enable)
            {
                products = products.Where(p => p.Enable);
            }

            if (filter.CategoryId > 0)
            {
                products = products.Where(p => p.CategoryId.Equals(filter.CategoryId));
            }

            if (filter.BrandId > 0)
            {
                products = products.Where(p => p.BrandId.Equals(filter.BrandId));
            }

            if (filter.ProducerId > 0)
            {
                products = products.Where(p => p.ProducerId.Equals(filter.ProducerId));
            }

            return products;
        }
    }
}
