﻿using StoreApp.BLL.DTO;
using StoreApp.Models.Store;
using AutoMapper;
using StoreApp.BLL.Services;
using StoreApp.Models.Filter;
using StoreApp.Models.Account;
using StoreApp.Models.Orders;
using StoreApp.Models.OrderDetails;
using System.Xml;
using System;
using System.Linq;
using StoreApp.BLL.Interfaces;

namespace StoreApp.Util
{
    public interface IWebMapper
    {
        IMapper Config { get; set; }
        IProductService productService { get; set; }

        ProductViewModel Map(ProductDTO productDTO);
        ProductDTO Map(XmlElement productXML);
        ProductDTO Map(ProductViewModel product);
    }

    public class WebMapper : IWebMapper
    {
        private UnitService unitService;
        private CategoryService categoryService;
        public IBrandService brandService { get; set; }
        public IProductService productService { get; set; }
        private ProducerService producerService;
        public IMapper Config { get; set; }

        public WebMapper(IBrandService brand)
        {
            productService = new ProductService();
            brandService = brand;
            unitService = new UnitService();
            categoryService = new CategoryService();
            producerService = new ProducerService();
            Config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<CreateProductViewModel, ProductDTO>();
                    cfg.CreateMap<ProductDTO, EditProductViewModel>();
                    cfg.CreateMap<EditProductViewModel, ProductDTO>();
                    cfg.CreateMap<ProductDTO, ProductViewModel>();
                    cfg.CreateMap<FilterProductsViewModel, FilterProductsDTO>();
                    cfg.CreateMap<UserDTO, UserViewModel>();
                    cfg.CreateMap<UserViewModel, UserDTO>();
                    cfg.CreateMap<UserLoginViewModel, UserDTO>();
                    cfg.CreateMap<UserRegistrationViewModel, UserDTO>();
                    cfg.CreateMap<UserDTO, UserEditViewModel>();
                    cfg.CreateMap<UserEditViewModel, UserDTO>();
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<OrderDetailDTO, OrderDetailsViewModel>();
                }).CreateMapper();
        }

        public ProductViewModel Map(ProductDTO productDTO)
        {
            return new ProductViewModel()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Price = productDTO.Price,
                Quantity = productDTO.Quantity,
                Picture = productDTO.Picture,
                Quality = productDTO.Quality,
                Enable = productDTO.Enable,
                Unit = unitService.Get(productDTO.UnitId)?.Name,
                Category = categoryService.Get(productDTO.CategoryId)?.Name,
                Brand = brandService.Get(productDTO.BrandId)?.Name,
                Producer = producerService.Get(productDTO.ProducerId)?.Name
            };
        }

        public ProductDTO Map(XmlElement productXML)
        {
            ProductDTO product = new ProductDTO();

            if (string.IsNullOrEmpty(productXML["name"].ToString()) || string.IsNullOrEmpty(productXML["price"].ToString()) ||
                string.IsNullOrEmpty(productXML["quantity"].ToString()) || string.IsNullOrEmpty(productXML["unit"].ToString()) ||
                string.IsNullOrEmpty(productXML["enable"].ToString()) || string.IsNullOrEmpty(productXML["category"].ToString()) ||
                string.IsNullOrEmpty(productXML["brand"].ToString()) || string.IsNullOrEmpty(productXML["producer"].ToString()))
            {
                return null;
            }
            product.Name = productXML["name"].InnerText;
            product.Price = decimal.Parse(productXML["price"].InnerText);
            product.Quantity = int.Parse(productXML["quantity"].InnerText);

            var unit = unitService.GetAll().FirstOrDefault(u => u.Name.Equals(productXML["unit"].InnerText));
            if (unit == null)
            {
                return null;
            }
            product.UnitId = unit.Id;

            product.Picture = productXML["picture"]?.InnerText;
            product.Quality = productXML["quality"]?.InnerText;
            product.Enable = Convert.ToBoolean(productXML["enable"].InnerText);

            var category = categoryService.GetAll().FirstOrDefault(c => c.Name.Equals(productXML["category"].InnerText));
            if (category == null)
            {
                return null;
            }
            product.CategoryId = category.Id;

            var brand = brandService.GetAll().FirstOrDefault(b => b.Name.Equals(productXML["brand"].InnerText));
            if (brand == null)
            {
                return null;
            }
            product.BrandId = brand.Id;

            var producer = producerService.GetAll().FirstOrDefault(p => p.Name.Equals(productXML["producer"].InnerText));
            if (producer == null)
            {
                return null;
            }
            product.ProducerId = producer.Id;

            return product;
        }

        public ProductDTO Map(ProductViewModel product)
        {
            if (string.IsNullOrEmpty(product.Name) ||
                product.Quantity < 0 || product.Price < 0 ||
                string.IsNullOrEmpty(product.Unit) || string.IsNullOrEmpty(product.Category) ||
                string.IsNullOrEmpty(product.Brand) || string.IsNullOrEmpty(product.Producer))
            {
                return null;
            }

            UnitDTO unit = unitService.GetAll().FirstOrDefault(u => u.Name.Equals(product.Unit));
            if (unit == null)
            {
                return null;
            }

            CategoryDTO category = categoryService.GetAll().FirstOrDefault(c => c.Name.Equals(product.Category));
            if (category == null)
            {
                return null;
            }

            BrandDTO brand = brandService.GetAll().FirstOrDefault(b => b.Name.Equals(product.Brand));
            if (brand == null)
            {
                return null;
            }

            ProducerDTO producer = producerService.GetAll().FirstOrDefault(p => p.Name.Equals(product.Producer));
            if (producer == null)
            {
                return null;
            }

            return new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Picture = product.Picture,
                Quality = product.Quality,
                Enable = product.Enable,
                UnitId = unit.Id,
                CategoryId = category.Id,
                BrandId = brand.Id,
                ProducerId = producer.Id
            };
        }
    }
}