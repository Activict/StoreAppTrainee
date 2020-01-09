using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class CategorySaver : ISaver
    {
        private CategoryService categoryService;
        private IEnumerable<CategoryDTO> categories;
        private int countNotUpload;
        private int countUpload;
        public string Message
        {
            get { return $"{countUpload} category's upload successful and {countNotUpload} is not"; }
        }

        public CategorySaver(IEnumerable<CategoryDTO> categories)
        {
            this.categories = categories;
            categoryService = new CategoryService();
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