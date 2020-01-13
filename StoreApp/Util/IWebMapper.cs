using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.Models.Store;
using System.Xml;

namespace StoreApp.Util
{
    public interface IWebMapper
    {
        IMapper Config { get; set; }
        IProductService ProductService { get; set; }
        IBrandService BrandService { get; set; }
        IUnitService UnitService { get; set; }
        ICategoryService CategoryService { get; set; }
        IProducerService ProducerService { get; set; }
        ProductViewModel Map(ProductDTO productDTO);
        ProductDTO Map(XmlElement productXML);
        ProductDTO Map(ProductViewModel product);
    }
}
