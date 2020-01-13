using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using System.Collections.Generic;

namespace StoreApp.Util
{
    public class BrandSaver : ISaver
    {
        private IBrandService brandService;
        private IEnumerable<BrandDTO> brands;
        private int countNotUpload;
        private int countUpload;
        public string Message
        {
            get { return $"{countUpload} brand's upload successful and {countNotUpload} is not"; }
        }

        public BrandSaver(IEnumerable<BrandDTO> brands, IWebMapper mapper)
        {
            this.brands = brands;
            brandService = mapper.BrandService;
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
