using StoreApp.BLL.DTO;
using System.Collections.Generic;

namespace StoreApp.BLL.Interfaces
{
    public interface IProductService
    {
        void Create(ProductDTO product);
        void Delete(int id);
        void Edit(ProductDTO product);
        ProductDTO Get(int id);
        IEnumerable<ProductDTO> GetAll();
        void Dispose();
        bool ValidateNewProduct(ProductDTO productDTO);
    }
}
