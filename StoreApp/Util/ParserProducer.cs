using Newtonsoft.Json;
using StoreApp.BLL.DTO;
using System.Collections.Generic;
using System.Xml;

namespace StoreApp.Util
{
    public class ParserProducer : IParser
    {
        private IWebMapper webMapper;
        private List<ProducerDTO> producersDTO = new List<ProducerDTO>();

        public ParserProducer(XmlElement xmlDocument, IWebMapper mapper)
        {
            webMapper = mapper;
            foreach (XmlElement producer in xmlDocument)
            {
                producersDTO.Add(new ProducerDTO() { Name = producer["name"].InnerText });
            }
        }

        public ParserProducer(string json, IWebMapper mapper)
        {
            webMapper = mapper;
            producersDTO = JsonConvert.DeserializeObject<List<ProducerDTO>>(json);
        }

        public ISaver GetSaver()
        {
            return new ProducerSaver(producersDTO, webMapper);
        }
    }
}