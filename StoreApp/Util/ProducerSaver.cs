using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class ProducerSaver : ISaver
    {
        private IProducerService producerService;
        private IEnumerable<ProducerDTO> producers;
        private int countNotUpload;
        private int countUpload;
        public string Message
        {
            get { return $"{countUpload} producer's upload successful and {countNotUpload} is not"; }
        }

        public ProducerSaver(IEnumerable<ProducerDTO> producers, IWebMapper mapper)
        {
            this.producers = producers;
            producerService = mapper.ProducerService;
        }

        public void Save()
        {
            foreach (var producer in producers)
            {
                if (!producerService.IsExistProducer(producer))
                {
                    producerService.Create(producer);
                    countUpload++;
                }
                else
                {
                    countNotUpload++;
                }
            }
        }
    }
}