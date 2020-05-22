using Newtonsoft.Json;
using StoreApp.BLL.DTO;
using System.Collections.Generic;
using System.Xml;

namespace StoreApp.Util
{
    public class ParserUnit : IParser
    {
        private IWebMapper webMapper;
        private List<UnitDTO> unitsDTO = new List<UnitDTO>();

        public ParserUnit(XmlElement xmlDocument, IWebMapper mapper)
        {
            webMapper = mapper;
            foreach (XmlElement unit in xmlDocument)
            {
                unitsDTO.Add(new UnitDTO() { Name = unit["name"].InnerText });
            }
        }

        public ParserUnit(string json, IWebMapper mapper)
        {
            webMapper = mapper;
            unitsDTO = JsonConvert.DeserializeObject<List<UnitDTO>>(json);
        }

        public ISaver GetSaver()
        {
            return new UnitSaver(unitsDTO, webMapper);
        }
    }
}