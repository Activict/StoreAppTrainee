using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    public class ProducerService : IProducerService
    {
        private IUnitOfWork DataBase { get; set; }

        private IMapper config;

        public ProducerService(IUnitOfWork uof)
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<Producer, ProducerDTO>()).CreateMapper();
            DataBase = uof;
        }

        public ProducerService()
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<Producer, ProducerDTO>()).CreateMapper();
            DataBase = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(ProducerDTO producer)
        {
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
            Producer producerDAL = config.Map<ProducerDTO, Producer>(producer);
            DataBase.Producers.Update(producerDAL);
            DataBase.Save();
        }

        public ProducerDTO Get(int id)
        {
            return config.Map<Producer, ProducerDTO>(DataBase.Producers.Get(id));
        }

        public IEnumerable<ProducerDTO> GetAll()
        {
            return config.Map<IEnumerable<Producer>, IEnumerable<ProducerDTO>>(DataBase.Producers.GetAll());
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
