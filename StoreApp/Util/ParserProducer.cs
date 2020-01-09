using Newtonsoft.Json;
using StoreApp.BLL.DTO;
using System.Collections.Generic;
using System.Xml;

namespace StoreApp.Util
{
    public class ParserProducer : IParser
    {
        private List<ProducerDTO> producersDTO = new List<ProducerDTO>();

        public ParserProducer(XmlElement xmlDocument)
        {
            foreach (XmlElement producer in xmlDocument)
            {
                producersDTO.Add(new ProducerDTO() { Name = producer["name"].InnerText });
            }
        }

        public ParserProducer(string json)
        {
            producersDTO = JsonConvert.DeserializeObject<List<ProducerDTO>>(json);
        }

        public ISaver GetSaver()
        {
            return new ProducerSaver(producersDTO);
        }
    }
}