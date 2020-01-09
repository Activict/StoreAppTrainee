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
        private IUnitOfWork database;
        private IMapper config;

        public BrandService(IUnitOfWork uof)
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Brand, BrandDTO>();
                cfg.CreateMap<BrandDTO, Brand>();
            }).CreateMapper();
            database = uof;
        }

        public BrandService()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Brand, BrandDTO>();
                cfg.CreateMap<BrandDTO, Brand>();
            }).CreateMapper();
            database = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(BrandDTO brand)
        {
            var brandDAL = config.Map<BrandDTO, Brand>(brand);
            database.Brands.Create(brandDAL);
            database.Save();
        }

        public void Create(string brand)
        {
            var brandDAL = new Brand() { Name = brand };
            database.Brands.Create(brandDAL);
            database.Save();
        }

        public void Delete(int id)
        {
            database.Brands.Delete(id);
            database.Save();
        }

        public void Edit(BrandDTO brand)
        {
            var brandDAL = config.Map<BrandDTO, Brand>(brand);
            database.Brands.Update(brandDAL);
            database.Save();
        }

        public BrandDTO Get(int id)
        {
            return config.Map<Brand, BrandDTO>(database.Brands.Get(id));
        }

        public IEnumerable<BrandDTO> GetAll()
        {
            return config.Map<IEnumerable<Brand>, List<BrandDTO>>(database.Brands.GetAll());
        }

        public int GetCountProductsByBrandId(int id)
        {
            return database.Products.GetAll().Count(p => p.BrandId.Equals(id));
        }

        public bool IsExistBrand(BrandDTO brandDTO)
        {
            var brands = database.Brands.GetAll();

            brands.ToList().ForEach(b => database.Brands.Detach(b));

            return brands.Any(b => b.Id != brandDTO.Id &&
                                    b.Name.Equals(brandDTO.Name));
        }

        public bool IsExistBrand(string brandDTO)
        {
            return database.Brands.GetAll().Any(b => b.Name.Equals(brandDTO));
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
