using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class BrandSaver : ISaver
    {
        private BrandService brandService;
        private IEnumerable<BrandDTO> Brands { get; set; }
        private int CountNotUpload { get; set; }
        private int CountUpload { get; set; }
        public string Message { get { return $"{CountUpload} brand's upload successful and {CountNotUpload} is not"; } }

        public BrandSaver(IEnumerable<BrandDTO> brands)
        {
            Brands = brands;
            brandService = new BrandService();
        }

        public void Save()
        {
            foreach (var brand in Brands)
            {
                if (!brandService.IsExistBrand(brand))
                {
                    brandService.Create(brand);
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
