using StoreApp.BLL.DTO;
using StoreApp.BLL.Services;
using System.Web;
using System.Xml;

namespace StoreApp.Util
{
    public class UploadXMLProducts : UploadXML
    {
        ProductService productService;
        public UploadXMLProducts(HttpPostedFileBase file) : base(file) 
        {
            productService = new ProductService();
        }
        
        public override bool IsValidateRoot()
        {
            if (IsValidateFile())
            {
                InitializeRoot();

                if (Root.Name == "products")
                {
                    return true;
                }
            }

            return false;
        }

        public override void SaveToDB()
        {
            if (IsValidateFile())
            {
                foreach (XmlElement productXML in Root)
                {
                    ProductDTO productDTO = webMapper.Map(productXML);

                    if (productDTO != null && productService.ValidateNewProduct(productDTO))
                    {
                        if (productDTO.Quantity.Equals(0))
                        {
                            productDTO.Enable = false;
                        }
                        productService.Create(productDTO);
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

}