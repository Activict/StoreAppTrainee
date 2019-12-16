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
            config = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDTO>()).CreateMapper();
            DataBase = uow;
        }

        public CategoryService()
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDTO>()).CreateMapper();
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
            Category categoryDAL = config.Map<Category>(category);
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

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
