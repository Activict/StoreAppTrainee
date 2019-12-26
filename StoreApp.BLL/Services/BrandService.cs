using AutoMapper;
using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using StoreApp.DAL.Entities;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.BLL.Services
{
    public class BrandService : IBrandService
    {
        private IUnitOfWork DataBase { get; set; }

        private IMapper config;

        public BrandService(IUnitOfWork uof)
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Brand, BrandDTO>();
                cfg.CreateMap<BrandDTO, Brand>();
            }).CreateMapper();
            DataBase = uof;
        }

        public BrandService()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Brand, BrandDTO>();
                cfg.CreateMap<BrandDTO, Brand>();
            }).CreateMapper();
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

        public int GetCountProductsByBrandId(int id)
        {
            return DataBase.Products.GetAll().Count(p => p.BrandId.Equals(id));
        }

        public bool IsExistBrand(BrandDTO brandDTO)
        {
            var brands = DataBase.Brands.GetAll();

            brands.ToList().ForEach(b => DataBase.Brands.Detach(b));

            return !brands.Any(b => b.Id != brandDTO.Id &&
                                    b.Name.Equals(brandDTO.Name));
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
