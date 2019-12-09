using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using System.Collections.Generic;
using StoreApp.DAL.Intefaces;
using StoreApp.DAL.Entities;
using AutoMapper;

namespace StoreApp.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        IUnitOfWork DataBase { get; set; }
        public CategoryService(IUnitOfWork uow)
        {
            DataBase = uow;
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CategoryDTO, Category>()).CreateMapper();
            Category categoryDAL = config.Map<Category>(category);
            DataBase.Categories.Update(categoryDAL);
            DataBase.Save();
        }

        public CategoryDTO Get(int id)
        {
            return new CategoryDTO(DataBase.Categories.Get(id));
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDTO>()).CreateMapper();
            return config.Map<IEnumerable<Category>, List<CategoryDTO>>(DataBase.Categories.GetAll());
        }
        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
