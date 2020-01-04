using StoreApp.BLL.DTO;
using System.Collections.Generic;
using System.Xml;

namespace StoreApp.Util
{
    public class ParserProduct : IParser
    {
        private WebMapper WebMapper = new WebMapper();

        private List<ProductDTO> productsDTO = new List<ProductDTO>();

        public ParserProduct(XmlElement xmlDocument)
        {
            foreach (XmlElement node in xmlDocument)
            {
                productsDTO.Add(WebMapper.Map(node));
            }
        }
        public ISaver GetSaver()
        {
            return new ProductSaver(productsDTO);
        }
    }
}