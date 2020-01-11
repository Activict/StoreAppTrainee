using Newtonsoft.Json;
using StoreApp.BLL.DTO;
using StoreApp.Models.Store;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace StoreApp.Util
{
    public class ParserProduct : IParser
    {
        private IWebMapper webMapper;

        private List<ProductDTO> productsDTO = new List<ProductDTO>();

        public ParserProduct(XmlElement xmlDocument, IWebMapper mapper)
        {
            webMapper = mapper;
            foreach (XmlElement node in xmlDocument)
            {
                productsDTO.Add(webMapper.Map(node));
            }
        }

        public ParserProduct(string json, IWebMapper mapper)
        {
            webMapper = mapper;
            var products = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(json);
            products.ToList().ForEach(p => productsDTO.Add(webMapper.Map(p)));
        }

        public ISaver GetSaver()
        {
            return new ProductSaver(productsDTO, webMapper);
        }
    }
}