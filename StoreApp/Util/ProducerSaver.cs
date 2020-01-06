using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class ProducerSaver : ISaver
    {
        private ProducerService ProducerService { get; set; }
        private IEnumerable<ProducerDTO> Producers { get; set; }
        private int CountNotUpload { get; set; }
        private int CountUpload { get; set; }
        public string Message
        {
            get { return $"{CountUpload} producer's upload successful and {CountNotUpload} is not"; }
        }

        public ProducerSaver(IEnumerable<ProducerDTO> producers)
        {
            Producers = producers;
            ProducerService = new ProducerService();
        }

        public void Save()
        {
            foreach (var producer in Producers)
            {
                if (!ProducerService.IsExistProducer(producer))
                {
                    ProducerService.Create(producer);
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