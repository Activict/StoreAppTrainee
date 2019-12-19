using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.BLL.Services
{
    public class UnitService : IUnitService
    {
        private IUnitOfWork DataBase { get; set; }
        private IMapper config;
        public UnitService(IUnitOfWork uof)
        {
            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Unit, UnitDTO>();
                cfg.CreateMap<UnitDTO, Unit>();
            }).CreateMapper();

            DataBase = uof;
        }

        public UnitService()
        {
            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Unit, UnitDTO>();
                cfg.CreateMap<UnitDTO, Unit>();
            }).CreateMapper();

            DataBase = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(UnitDTO unit)
        {
            Unit unitDAL = config.Map<UnitDTO, Unit>(unit);
            DataBase.Units.Create(unitDAL);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.Units.Delete(id);
            DataBase.Save();
        }

        public void Detach(UnitDTO unit)
        {
            throw new NotImplementedException();
        }

        public void Edit(UnitDTO unit)
        {
            Unit unitDAL = config.Map<UnitDTO, Unit>(unit);
            DataBase.Units.Update(unitDAL);
            DataBase.Save();
        }

        public UnitDTO Get(int id)
        {
            return config.Map<Unit, UnitDTO>(DataBase.Units.Get(id));
        }

        public IEnumerable<UnitDTO> GetAll()
        {
            return config.Map<IEnumerable<Unit>, List<UnitDTO>>(DataBase.Units.GetAll());
        }

        public int GetCountProductsByUnitId(int id)
        {
            return DataBase.Products.GetAll().Count(p => p.UnitId.Equals(id));
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }

    }
}
