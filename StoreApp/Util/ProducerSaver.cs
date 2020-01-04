using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class ProducerSaver : ISaver
    {
        private ProducerService producerService;
        private IEnumerable<ProducerDTO> Producers{ get; set; }
        private int CountNotUpload { get; set; }
        private int CountUpload { get; set; }
        public string Message { get { return $"{CountUpload} producer's upload successful and {CountNotUpload} is not"; } }

        public ProducerSaver(IEnumerable<ProducerDTO> producers)
        {
            Producers = producers;
            producerService = new ProducerService();
        }

        public void Save()
        {
            foreach (var producer in Producers)
            {
                if (!producerService.IsExistProducer(producer))
                {
                    producerService.Create(producer);
                    CountUpload++;
                }
                else
                {
                    CountNotUpload++;
                }
            }
        }
    }
}