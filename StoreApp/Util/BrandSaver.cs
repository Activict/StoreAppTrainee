using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class BrandSaver : ISaver
    {
        private BrandService BrandService { get; set; }
        private IEnumerable<BrandDTO> Brands { get; set; }
        private int CountNotUpload { get; set; }
        private int CountUpload { get; set; }
        public string Message
        {
            get { return $"{CountUpload} brand's upload successful and {CountNotUpload} is not"; }
        }

        public BrandSaver(IEnumerable<BrandDTO> brands)
        {
            Brands = brands;
            BrandService = new BrandService();
        }

        public void Save()
        {
            foreach (var brand in Brands)
            {
                if (!BrandService.IsExistBrand(brand))
                {
                    BrandService.Create(brand);
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
