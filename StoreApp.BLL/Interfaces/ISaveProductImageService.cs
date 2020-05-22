using StoreApp.BLL.DTO;
using System.Web;

namespace StoreApp.BLL.Interfaces
{
    public interface ISaveProductImageService
    {
        void SaveImage(ProductDTO productDTO, HttpPostedFileBase file);
    }
}
