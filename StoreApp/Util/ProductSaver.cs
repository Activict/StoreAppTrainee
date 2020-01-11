using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class ProductSaver : ISaver
    {
        private IProductService productService;
        private IEnumerable<ProductDTO> products;
        private int countNotUpload;
        private int countUpload;
        public string Message
        {
            get { return $"{countUpload} product's upload successful and {countNotUpload} is not"; }
        }

        public ProductSaver(IEnumerable<ProductDTO> products, IWebMapper mapper)
        {
            this.products = products;
            productService = mapper.productService;
        }

        public void Save()
        {
            foreach (var productDTO in products)
            {
                if (productDTO != null && productService.ValidateNewProduct(productDTO))
                {
                    productService.Create(productDTO);
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