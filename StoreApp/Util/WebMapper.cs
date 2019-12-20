using StoreApp.BLL.DTO;
using StoreApp.Models.Store;
using AutoMapper;
using StoreApp.BLL.Services;
using StoreApp.Models.Filter;

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
    }
}