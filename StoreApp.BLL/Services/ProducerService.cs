using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    public class ProducerService : IProducerService
    {
        IUnitOfWork DataBase { get; set; }
        public ProducerService(IUnitOfWork uof)
        {
            DataBase = uof;
        }
        public void Create(ProducerDTO producer)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Producer, ProducerDTO>()).CreateMapper();
            Producer producerDAL = config.Map<ProducerDTO, Producer>(producer);
            DataBase.Producers.Create(producerDAL);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.Producers.Delete(id);
            DataBase.Save();
        }

        public void Edit(ProducerDTO producer)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProducerDTO, Producer>()).CreateMapper();
            Producer producerDAL = config.Map<ProducerDTO, Producer>(producer);
            DataBase.Producers.Update(producerDAL);
            DataBase.Save();
        }

        public ProducerDTO Get(int id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Producer, ProducerDTO>()).CreateMapper();
            return config.Map<Producer, ProducerDTO>(DataBase.Producers.Get(id));
        }

        public IEnumerable<ProducerDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Producer, ProducerDTO>()).CreateMapper();
            return config.Map<IEnumerable<Producer>, IEnumerable<ProducerDTO>>(DataBase.Producers.GetAll());
        }
        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
