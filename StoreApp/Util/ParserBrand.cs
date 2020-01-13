using Newtonsoft.Json;
using StoreApp.BLL.DTO;
using System.Collections.Generic;
using System.Xml;

namespace StoreApp.Util
{
    public class ParserBrand : IParser
    {
        private IWebMapper webMapper;
        private List<BrandDTO> brandsDTO = new List<BrandDTO>();

        public ParserBrand(XmlElement xmlDocument, IWebMapper mapper)
        {
            webMapper = mapper;
            foreach (XmlElement brand in xmlDocument)
            {
                brandsDTO.Add(new BrandDTO() { Name = brand["name"].InnerText });
            }
        }

        public ParserBrand(string json, IWebMapper mapper)
        {
            webMapper = mapper;
            brandsDTO = JsonConvert.DeserializeObject<List<BrandDTO>>(json);
        }

        public ISaver GetSaver()
        {
            return new BrandSaver(brandsDTO, webMapper);
        }
    }
}