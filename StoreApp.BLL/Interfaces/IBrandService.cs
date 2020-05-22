using StoreApp.BLL.DTO;
using System.Collections.Generic;

namespace StoreApp.BLL.Interfaces
{
    public interface IBrandService
    {
        void Create(BrandDTO brand);
        void Delete(int id);
        void Edit(BrandDTO brand);
        BrandDTO Get(int id);
        IEnumerable<BrandDTO> GetAll();
        void Dispose();
        bool IsExistBrand(BrandDTO brandDTO);
        int GetCountProductsByBrandId(int id);
    }
}
