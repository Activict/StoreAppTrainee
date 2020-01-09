using Newtonsoft.Json;
using StoreApp.BLL.DTO;
using System.Collections.Generic;
using System.Xml;

namespace StoreApp.Util
{
    public class ParserCategory : IParser
    {
        private List<CategoryDTO> categoriesDTO = new List<CategoryDTO>();

        public ParserCategory(XmlElement xmlDocument)
        {
            foreach (XmlElement category in xmlDocument)
            {
                categoriesDTO.Add(new CategoryDTO() { Name = category["name"].InnerText });
            }
        }

        public ParserCategory(string json)
        {
            categoriesDTO = JsonConvert.DeserializeObject<List<CategoryDTO>>(json);
        }

        public ISaver GetSaver()
        {
            return new CategorySaver(categoriesDTO);
        }
    }
}