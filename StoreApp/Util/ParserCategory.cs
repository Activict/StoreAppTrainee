using Newtonsoft.Json;
using StoreApp.BLL.DTO;
using System.Collections.Generic;
using System.Xml;

namespace StoreApp.Util
{
    public class ParserCategory : IParser
    {
        private IWebMapper webMapper;
        private List<CategoryDTO> categoriesDTO = new List<CategoryDTO>();

        public ParserCategory(XmlElement xmlDocument, IWebMapper mapper)
        {
            webMapper = mapper;
            foreach (XmlElement category in xmlDocument)
            {
                categoriesDTO.Add(new CategoryDTO() { Name = category["name"].InnerText });
            }
        }

        public ParserCategory(string json, IWebMapper mapper)
        {
            webMapper = mapper;
            categoriesDTO = JsonConvert.DeserializeObject<List<CategoryDTO>>(json);
        }

        public ISaver GetSaver()
        {
            return new CategorySaver(categoriesDTO, webMapper);
        }
    }
}