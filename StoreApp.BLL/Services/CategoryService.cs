using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using System.Collections.Generic;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Entities;
using AutoMapper;
using StoreApp.DAL.Repositories;
using System.Linq;

namespace StoreApp.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork DataBase { get; set; }

        private IMapper config;

        public CategoryService(IUnitOfWork uow)
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<CategoryDTO, Category>();
            }).CreateMapper();
            DataBase = uow;
        }

        public CategoryService()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<CategoryDTO, Category>();
            }).CreateMapper();
            DataBase = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(CategoryDTO category)
        {
            var categoryBL = new Category()
            {
                Id = category.Id,
                Name = category.Name
            };
            DataBase.Categories.Create(categoryBL);
            DataBase.Save();
        }

        public void Delete(int id)
        {
            DataBase.Categories.Delete(id);
            DataBase.Save();
        }

        public void Edit(CategoryDTO category)
        {
            Category categoryDAL = config.Map<CategoryDTO, Category>(category);
            DataBase.Categories.Update(categoryDAL);
            DataBase.Save();
        }

        public CategoryDTO Get(int id)
        {
            return config.Map<Category, CategoryDTO>(DataBase.Categories.Get(id));
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            return config.Map<IEnumerable<Category>, List<CategoryDTO>>(DataBase.Categories.GetAll());
        }

        public int GetCountProductsByCategoryId(int id)
        {
            return DataBase.Products.GetAll().Count(p => p.CategoryId.Equals(id));
        }

        public bool IsExistCategory(CategoryDTO categoryDTO)
        {
            var categories = DataBase.Categories.GetAll();

            categories.ToList().ForEach(c => DataBase.Categories.Detach(c));

            return !categories.Any(c => c.Id != categoryDTO.Id &&
                                        c.Name.Equals(categoryDTO.Name));
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
