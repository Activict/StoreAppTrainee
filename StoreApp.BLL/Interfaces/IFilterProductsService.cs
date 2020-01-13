using StoreApp.BLL.DTO;
using System.Collections.Generic;

namespace StoreApp.BLL.Interfaces
{
    public interface IFilterProductsService
    {
        IEnumerable<ProductDTO> GetProductsDTOFiltered(FilterProductsDTO filter);
    }
}
