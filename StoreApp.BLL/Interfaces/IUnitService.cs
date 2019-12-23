using StoreApp.BLL.DTO;
using System.Collections.Generic;

namespace StoreApp.BLL.Interfaces
{
    interface IUnitService
    {
        void Create(UnitDTO unit);
        void Delete(int id);
        void Edit(UnitDTO unit);
        UnitDTO Get(int id);
        IEnumerable<UnitDTO> GetAll();
        void Detach(UnitDTO unit);
        void Dispose();
    }
}
