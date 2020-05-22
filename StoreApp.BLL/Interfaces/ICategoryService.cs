using StoreApp.BLL.DTO;
using System.Collections.Generic;

namespace StoreApp.BLL.Interfaces
{
    public interface ICategoryService
    {
        void Create(CategoryDTO category);
        void Delete(int id);
        void Edit(CategoryDTO category);
        CategoryDTO Get(int id);
        IEnumerable<CategoryDTO> GetAll();
        void Dispose();
        int GetCountProductsByCategoryId(int id);
        bool IsExistCategory(CategoryDTO categoryDTO);
    }
}
