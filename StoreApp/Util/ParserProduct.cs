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
        private WebMapper webMapper = new WebMapper();

        private List<ProductDTO> productsDTO = new List<ProductDTO>();

        public ParserProduct(XmlElement xmlDocument)
        {
            foreach (XmlElement node in xmlDocument)
            {
                productsDTO.Add(webMapper.Map(node));
            }
        }

        public ParserProduct(string json)
        {
            var products = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(json);

            products.ToList().ForEach(p => productsDTO.Add(webMapper.Map(p)));
        }

        public ISaver GetSaver()
        {
            return new ProductSaver(productsDTO);
        }
    }
}