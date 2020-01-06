using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class UnitSaver : ISaver
    {
        private UnitService UnitService { get; set; }
        private IEnumerable<UnitDTO> Units { get; set; }
        private int CountNotUpload { get; set; }
        private int CountUpload { get; set; }
        public string Message
        {
            get { return $"{CountUpload} unit's upload successful and {CountNotUpload} is not"; }
        }

        public UnitSaver(IEnumerable<UnitDTO> units)
        {
            Units = units;
            UnitService = new UnitService();
        }

        public void Save()
        {
            foreach (var unit in Units)
            {
                if (!UnitService.IsExistUnit(unit))
                {
                    UnitService.Create(unit);
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
