using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.Models.Account;
using StoreApp.Models.Filter;
using StoreApp.Models.OrderDetails;
using StoreApp.Models.Orders;
using StoreApp.Models.Store;
using System;
using System.Linq;
using System.Xml;

namespace StoreApp.Util
{
    public class WebMapper : IWebMapper
    {
        public IUnitService UnitService { get; set; }
        public ICategoryService CategoryService { get; set; }
        public IBrandService BrandService { get; set; }
        public IProductService ProductService { get; set; }
        public IProducerService ProducerService { get; set; }
        public IMapper Config { get; set; }

        public WebMapper(
            IBrandService brand, 
            IProductService product,
            IUnitService unit,
            ICategoryService category,
            IProducerService producer)
        {
            ProductService = product;
            BrandService = brand;
            UnitService = unit;
            CategoryService = category;
            ProducerService = producer;
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
                Unit = UnitService.Get(productDTO.UnitId)?.Name,
                Category = CategoryService.Get(productDTO.CategoryId)?.Name,
                Brand = BrandService.Get(productDTO.BrandId)?.Name,
                Producer = ProducerService.Get(productDTO.ProducerId)?.Name
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

            var unit = UnitService.GetAll().FirstOrDefault(u => u.Name.Equals(productXML["unit"].InnerText));
            if (unit == null)
            {
                return null;
            }
            product.UnitId = unit.Id;

            product.Picture = productXML["picture"]?.InnerText;
            product.Quality = productXML["quality"]?.InnerText;
            product.Enable = Convert.ToBoolean(productXML["enable"].InnerText);

            var category = CategoryService.GetAll().FirstOrDefault(c => c.Name.Equals(productXML["category"].InnerText));
            if (category == null)
            {
                return null;
            }
            product.CategoryId = category.Id;

            var brand = BrandService.GetAll().FirstOrDefault(b => b.Name.Equals(productXML["brand"].InnerText));
            if (brand == null)
            {
                return null;
            }
            product.BrandId = brand.Id;

            var producer = ProducerService.GetAll().FirstOrDefault(p => p.Name.Equals(productXML["producer"].InnerText));
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

            UnitDTO unit = UnitService.GetAll().FirstOrDefault(u => u.Name.Equals(product.Unit));
            if (unit == null)
            {
                return null;
            }

            CategoryDTO category = CategoryService.GetAll().FirstOrDefault(c => c.Name.Equals(product.Category));
            if (category == null)
            {
                return null;
            }

            BrandDTO brand = BrandService.GetAll().FirstOrDefault(b => b.Name.Equals(product.Brand));
            if (brand == null)
            {
                return null;
            }

            ProducerDTO producer = ProducerService.GetAll().FirstOrDefault(p => p.Name.Equals(product.Producer));
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