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
        private IUnitOfWork database;
        private IMapper config;

        public CategoryService(IUnitOfWork uow)
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<CategoryDTO, Category>();
            }).CreateMapper();
            database = uow;
        }

        public CategoryService()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<CategoryDTO, Category>();
            }).CreateMapper();
            database = new EFUnitOfWork("DefaultConnection");
        }

        public void Create(CategoryDTO category)
        {
            var categoryBL = new Category() { Name = category.Name };
            database.Categories.Create(categoryBL);
            database.Save();
        }

        public void Create(string category)
        {
            var categoryDAL = new Category() { Name = category };
            database.Categories.Create(categoryDAL);
            database.Save();
        }

        public void Delete(int id)
        {
            database.Categories.Delete(id);
            database.Save();
        }

        public void Edit(CategoryDTO category)
        {
            var categoryDAL = config.Map<CategoryDTO, Category>(category);
            database.Categories.Update(categoryDAL);
            database.Save();
        }

        public CategoryDTO Get(int id)
        {
            return config.Map<Category, CategoryDTO>(database.Categories.Get(id));
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            return config.Map<IEnumerable<Category>, List<CategoryDTO>>(database.Categories.GetAll());
        }

        public int GetCountProductsByCategoryId(int id)
        {
            return database.Products.GetAll().Count(p => p.CategoryId.Equals(id));
        }

        public bool IsExistCategory(CategoryDTO categoryDTO)
        {
            var categories = database.Categories.GetAll();

            categories.ToList().ForEach(c => database.Categories.Detach(c));

            return categories.Any(c => c.Id != categoryDTO.Id &&
                                        c.Name.Equals(categoryDTO.Name));
        }

        public bool IsExistCategory(string category)
        {
            return database.Categories.GetAll().Any(c => c.Name.Equals(category));
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
