using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class CategorySaver : ISaver
    {
        private ICategoryService categoryService;
        private IEnumerable<CategoryDTO> categories;
        private int countNotUpload;
        private int countUpload;
        public string Message
        {
            get { return $"{countUpload} category's upload successful and {countNotUpload} is not"; }
        }

        public CategorySaver(IEnumerable<CategoryDTO> categories, IWebMapper mapper)
        {
            this.categories = categories;
            categoryService = mapper.CategoryService;
        }

        public void Save()
        {
            foreach (var category in categories)
            {
                if (!categoryService.IsExistCategory(category))
                {
                    categoryService.Create(category);
                    countUpload++;
                }
                else
                {
                    countNotUpload++;
                }
            }
        }
    }
}