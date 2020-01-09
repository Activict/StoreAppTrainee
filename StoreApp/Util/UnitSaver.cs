using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class UnitSaver : ISaver
    {
        private UnitService unitService { get; set; }
        private IEnumerable<UnitDTO> units { get; set; }
        private int countNotUpload { get; set; }
        private int countUpload { get; set; }
        public string Message
        {
            get { return $"{countUpload} unit's upload successful and {countNotUpload} is not"; }
        }

        public UnitSaver(IEnumerable<UnitDTO> units)
        {
            this.units = units;
            unitService = new UnitService();
        }

        public void Save()
        {
            foreach (var unit in units)
            {
                if (!unitService.IsExistUnit(unit))
                {
                    unitService.Create(unit);
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
