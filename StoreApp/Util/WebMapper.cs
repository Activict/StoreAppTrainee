using StoreApp.BLL.DTO;
using StoreApp.Models.Store;
using AutoMapper;
using StoreApp.BLL.Services;

namespace StoreApp.Util
{
    public class WebMapper
    {
        private ProductService productService;
        private CategoryService categoryService;
        private BrandService brandService;
        private ProducerService producerService;
        public IMapper config;

        public WebMapper()
        {
            productService = new ProductService();
            categoryService = new CategoryService();
            brandService = new BrandService();
            producerService = new ProducerService();
            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CreateProductViewModel, ProductDTO>();
                cfg.CreateMap<ProductDTO, EditProductViewModel>();
                cfg.CreateMap<EditProductViewModel, ProductDTO>();
                cfg.CreateMap<ProductDTO, ProductViewModel>();
            }).CreateMapper();
        }

        public ProductViewModel Map(ProductDTO productDTO)
        {
            return  new ProductViewModel()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Price = productDTO.Price,
                Quantity = productDTO.Quantity,
                Unit = productDTO.Unit,
                Picture = productDTO.Picture,
                Quality = productDTO.Quality,
                Enable = productDTO.Enable,
                Category = categoryService.Get(productDTO.CategoryId)?.Name,
                Brand = brandService.Get(productDTO.BrandId)?.Name,
                Producer = producerService.Get(productDTO.ProducerId)?.Name
            };
        }
    }
}