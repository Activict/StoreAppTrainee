using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class CategorySaver : ISaver
    {
        private CategoryService categoryService;
        private IEnumerable<CategoryDTO> Categories { get; set; }
        private int CountNotUpload { get; set; }
        private int CountUpload { get; set; }
        public string Message { get { return $"{CountUpload} category's upload successful and {CountNotUpload} is not"; } }

        public CategorySaver(IEnumerable<CategoryDTO> categories)
        {
            Categories = categories;
            categoryService = new CategoryService();
        }

        public void Save()
        {
            foreach (var category in Categories)
            {
                if (!categoryService.IsExistCategory(category))
                {
                    categoryService.Create(category);
                    CountUpload++;
                }
                else
                {
                    CountNotUpload++;
                }
            }
        }
    }
}