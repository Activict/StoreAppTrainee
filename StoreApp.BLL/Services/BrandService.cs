using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using System.Collections.Generic;

namespace StoreApp.BLL.Services
{
    public class BrandService : IBrandService
    {
        IUnitOfWork DataBase { get; set; }
        public BrandService(IUnitOfWork uof)
        {
            DataBase = uof;
        }
        public void Create(BrandDTO brand)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDTO>()).CreateMapper();
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDTO>()).CreateMapper();
            Brand brandDAL = config.Map<BrandDTO, Brand>(brand);
            DataBase.Brands.Update(brandDAL);
            DataBase.Save();
        }

        public BrandDTO Get(int id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDTO>()).CreateMapper();
            return config.Map<Brand, BrandDTO>(DataBase.Brands.Get(id));
        }

        public IEnumerable<BrandDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDTO>()).CreateMapper();
            return config.Map<IEnumerable<Brand>, List<BrandDTO>>(DataBase.Brands.GetAll());
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
