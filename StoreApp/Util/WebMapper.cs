using StoreApp.BLL.DTO;
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

namespace StoreApp.Util
{
    public class WebMapper
    {
        private UnitService unitService;
        private ProductService productService;
        private CategoryService categoryService;
        private BrandService brandService;
        private ProducerService producerService;
        public IMapper config;

        public WebMapper()
        {
            unitService = new UnitService();
            productService = new ProductService();
            categoryService = new CategoryService();
            brandService = new BrandService();
            producerService = new ProducerService();
            config = new MapperConfiguration(
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
    }
}