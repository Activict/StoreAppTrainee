using StoreApp.BLL.DTO;
using System.Collections.Generic;

namespace StoreApp.BLL.Interfaces
{
    public interface IProducerService
    {
        void Create(ProducerDTO producer);
        void Delete(int id);
        void Edit(ProducerDTO producer);
        ProducerDTO Get(int id);
        IEnumerable<ProducerDTO> GetAll();
        bool IsExistProducer(ProducerDTO producerDTO);
        int GetCountProductsByProducerId(int id);
        void Dispose();
    }
}
