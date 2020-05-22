using StoreApp.BLL.DTO;
using System.Collections.Generic;

namespace StoreApp.BLL.Interfaces
{
    public interface IUnitService
    {
        void Create(UnitDTO unitDTO);
        void Delete(int id);
        void Edit(UnitDTO unit);
        UnitDTO Get(int id);
        IEnumerable<UnitDTO> GetAll();
        void Detach(UnitDTO unit);
        void Dispose();
        bool IsExistUnit(UnitDTO unitDTO);
        int GetCountProductsByUnitId(int id);
    }
}
