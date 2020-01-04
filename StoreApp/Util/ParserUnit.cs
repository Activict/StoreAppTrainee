using StoreApp.BLL.DTO;
using System.Collections.Generic;
using System.Xml;

namespace StoreApp.Util
{
    public class ParserUnit : IParser
    {
        private List<UnitDTO> unitsDTO = new List<UnitDTO>();

        public ParserUnit(XmlElement xmlDocument)
        {
            foreach (XmlElement unit in xmlDocument)
            {
                unitsDTO.Add(new UnitDTO() { Name = unit["name"].InnerText });
            }
        }
        public ISaver GetSaver()
        {
            return new UnitSaver(unitsDTO);
        }
    }
}