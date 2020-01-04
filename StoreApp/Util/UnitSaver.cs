using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class UnitSaver : ISaver
    {
        private UnitService unitService;
        private IEnumerable<UnitDTO> Units { get; set; }
        private int CountNotUpload { get; set; }
        private int CountUpload { get; set; }
        public string Message { get { return $"{CountUpload} unit's upload successful and {CountNotUpload} is not"; } }

        public UnitSaver(IEnumerable<UnitDTO> units)
        {
            Units = units;
            unitService = new UnitService();
        }

        public void Save()
        {
            foreach (var unit in Units)
            {
                if (!unitService.IsExistUnit(unit))
                {
                    unitService.Create(unit);
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
