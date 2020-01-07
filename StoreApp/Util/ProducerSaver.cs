using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class ProducerSaver : ISaver
    {
        private ProducerService producerService;
        private IEnumerable<ProducerDTO> producers;
        private int countNotUpload;
        private int countUpload;
        public string Message
        {
            get { return $"{countUpload} producer's upload successful and {countNotUpload} is not"; }
        }

        public ProducerSaver(IEnumerable<ProducerDTO> producers)
        {
            this.producers = producers;
            producerService = new ProducerService();
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