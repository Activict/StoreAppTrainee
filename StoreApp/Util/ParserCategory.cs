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
        public ISaver GetSaver()
        {
            return new CategorySaver(categoriesDTO);
        }
    }
}