using StoreApp.BLL.DTO;
using StoreApp.BLL.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace StoreApp.BLL.Services
{
    public class SaveProductImageService : ISaveProductImageService
    {
        private IProductService productService;

        public SaveProductImageService(IProductService product)
        {
            productService = product;
        }

        public void SaveImage(ProductDTO productDTO, HttpPostedFileBase file)
        {
            int productId = productService.GetAll()
                    .FirstOrDefault(p => p.Name.Equals(productDTO.Name) &&
                                         p.BrandId.Equals(productDTO.BrandId) &&
                                         p.ProducerId.Equals(productDTO.ProducerId)).Id;

            var pathStringPictures = new DirectoryInfo(string.Format($"{AppDomain.CurrentDomain.BaseDirectory}Pictures\\Products"));
            var pathStringProductsById = Path.Combine(pathStringPictures.ToString(), productId.ToString());

            if (Directory.Exists(pathStringProductsById))
            {
                foreach (var fileForDel in new DirectoryInfo(pathStringProductsById).GetFiles())
                {
                    fileForDel.Delete();
                }
            }
            else
            {
                Directory.CreateDirectory(pathStringProductsById);
            }

            var pathSavePicture = string.Format($"{pathStringProductsById}\\{file.FileName}");
            var pathSavePicturePreview = string.Format($"{pathStringProductsById}\\{ "preview_" + file.FileName}");

            WebImage pic = new WebImage(file.InputStream);
            pic.Resize(200, 200).Crop(1, 1);
            pic.Save(pathSavePicture);
            pic.Resize(30, 30).Crop(1, 1);
            pic.Save(pathSavePicturePreview);
        }
    }
}
