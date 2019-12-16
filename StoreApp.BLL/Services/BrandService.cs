using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    public class BrandService : IBrandService
    {
        private IUnitOfWork DataBase { get; set; }

        private IMapper config;

        public BrandService(IUnitOfWork uof)
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDTO>()).CreateMapper();
            DataBase = uof;
        }

        public BrandService()
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDTO>()).CreateMapper();
            DataBase = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(BrandDTO brand)
        {
            Brand brandDAL = config.Map<BrandDTO, Brand>(brand);
            DataBase.Brands.Create(brandDAL);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.Brands.Delete(id);
            DataBase.Save();
        }

        public void Edit(BrandDTO brand)
        {
            Brand brandDAL = config.Map<BrandDTO, Brand>(brand);
            DataBase.Brands.Update(brandDAL);
            DataBase.Save();
        }

        public BrandDTO Get(int id)
        {
            return config.Map<Brand, BrandDTO>(DataBase.Brands.Get(id));
        }

        public IEnumerable<BrandDTO> GetAll()
        {
            return config.Map<IEnumerable<Brand>, List<BrandDTO>>(DataBase.Brands.GetAll());
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
