using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class BrandSaver : ISaver
    {
        private BrandService brandService;
        private IEnumerable<BrandDTO> brands;
        private int countNotUpload;
        private int countUpload;
        public string Message
        {
            get { return $"{countUpload} brand's upload successful and {countNotUpload} is not"; }
        }

        public BrandSaver(IEnumerable<BrandDTO> brands)
        {
            this.brands = brands;
            brandService = new BrandService();
        }

        public void Save()
        {
            foreach (var brand in brands)
            {
                if (!brandService.IsExistBrand(brand))
                {
                    brandService.Create(brand);
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
