using StoreApp.BLL.DTO;
using System.Collections.Generic;
using System.Xml;

namespace StoreApp.Util
{
    public class ParserBrand : IParser
    {
        private List<BrandDTO> brandsDTO = new List<BrandDTO>();

        public ParserBrand(XmlElement xmlDocument)
        {
            foreach (XmlElement brand in xmlDocument)
            {
                brandsDTO.Add(new BrandDTO() { Name = brand["name"].InnerText });
            }
        }
        public ISaver GetSaver()
        {
            return new BrandSaver(brandsDTO);
        }
    }
}