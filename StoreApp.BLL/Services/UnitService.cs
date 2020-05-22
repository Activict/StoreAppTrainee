using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.BLL.Services
{
    public class UnitService : IUnitService
    {
        private IUnitOfWork database;
        private IMapper config;

        public UnitService(IUnitOfWork uof)
        {
            database = uof;
            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Unit, UnitDTO>();
                cfg.CreateMap<UnitDTO, Unit>();
            }).CreateMapper();
        }

        public void Create(UnitDTO unit)
        {
            var unitDAL = config.Map<UnitDTO, Unit>(unit);
            database.Units.Create(unitDAL);
            database.Save();
        }

        public void Create(string unit)
        {
            var unitDAL = new Unit() { Name = unit };
            database.Units.Create(unitDAL);
            database.Save();
        }

        public void Delete(int id)
        {
            database.Units.Delete(id);
            database.Save();
        }

        public void Detach(UnitDTO unit)
        {
            throw new NotImplementedException();
        }

        public void Edit(UnitDTO unit)
        {
            var unitDAL = config.Map<UnitDTO, Unit>(unit);
            database.Units.Update(unitDAL);
            database.Save();
        }

        public UnitDTO Get(int id)
        {
            return config.Map<Unit, UnitDTO>(database.Units.Get(id));
        }

        public IEnumerable<UnitDTO> GetAll()
        {
            return config.Map<IEnumerable<Unit>, List<UnitDTO>>(database.Units.GetAll());
        }

        public int GetCountProductsByUnitId(int id)
        {
            return database.Products.GetAll().Count(p => p.UnitId.Equals(id));
        }

        public bool IsExistUnit(UnitDTO unitDTO)
        {
            var units = database.Units.GetAll();

            units.ToList().ForEach(p => database.Units.Detach(p));

            return units.Any(p => p.Id != unitDTO.Id &&
                                   p.Name.Equals(unitDTO.Name));
        }

        public bool IsExistUnit(string unit)
        {
            return database.Units.GetAll().Any(p => p.Name.Equals(unit));
        }

        public void Dispose()
        {
            database.Dispose();
        }

    }
}
