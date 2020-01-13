using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.BLL.Services
{
    public class ProducerService : IProducerService
    {
        private IUnitOfWork database;
        private IMapper config;

        public ProducerService(IUnitOfWork uof)
        {
            database = uof;
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Producer, ProducerDTO>();
                cfg.CreateMap<ProducerDTO, Producer>();
            }).CreateMapper();
        }

        public void Create(ProducerDTO producer)
        {
            var producerDAL = config.Map<ProducerDTO, Producer>(producer);
            database.Producers.Create(producerDAL);
            database.Save();
        }
        public void Create(string producer)
        {
            var producerDAL = new Producer() { Name = producer};
            database.Producers.Create(producerDAL);
            database.Save();
        }

        public void Delete(int id)
        {
            database.Producers.Delete(id);
            database.Save();
        }

        public void Edit(ProducerDTO producer)
        {
            var producerDAL = config.Map<ProducerDTO, Producer>(producer);
            database.Producers.Update(producerDAL);
            database.Save();
        }

        public ProducerDTO Get(int id)
        {
            return config.Map<Producer, ProducerDTO>(database.Producers.Get(id));
        }

        public IEnumerable<ProducerDTO> GetAll()
        {
            return config.Map<IEnumerable<Producer>, IEnumerable<ProducerDTO>>(database.Producers.GetAll());
        }

        public int GetCountProductsByProducerId(int id)
        {
            return database.Products.GetAll().Count(p => p.ProducerId.Equals(id));
        }

        public bool IsExistProducer(ProducerDTO producerDTO)
        {
            var producers = database.Producers.GetAll();

            producers.ToList().ForEach(p => database.Producers.Detach(p));

            return producers.Any(p => p.Id != producerDTO.Id &&
                                       p.Name.Equals(producerDTO.Name));
        }

        public bool IsExistProducer(string producer)
        {
            return database.Producers.GetAll().Any(p => p.Name.Equals(producer));
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
