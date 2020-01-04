using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class ProductSaver : ISaver
    {
        private ProductService productService;
        private IEnumerable<ProductDTO> Products { get; set; }
        private int CountNotUpload { get; set; }
        private int CountUpload { get; set; }
        public string Message { get { return $"{CountUpload} product's upload successful and {CountNotUpload} is not"; } }

        public ProductSaver(IEnumerable<ProductDTO> products)
        {
            Products = products;
            productService = new ProductService();
        }

        public void Save()
        {
            foreach (var productDTO in Products)
            {
                if (productDTO != null && productService.ValidateNewProduct(productDTO))
                {
                    productService.Create(productDTO);
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