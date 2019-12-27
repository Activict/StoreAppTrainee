using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StoreApp.BLL.Services
{
    public class SaveXMLService
    {
        public void SaveXML(HttpPostedFileBase file, string folder)
        {
            var pathStringContent = new DirectoryInfo(string.Format($"{AppDomain.CurrentDomain.BaseDirectory}Content\\"));
            var pathStringContentXML = Path.Combine(pathStringContent.ToString(), "XMLUploads");
            var pathStringXMLFolder = Path.Combine(pathStringContentXML.ToString(), folder);

            if (!Directory.Exists(pathStringXMLFolder))
            {
                Directory.CreateDirectory(pathStringXMLFolder);
            }

            var pathSaveXMLFile = string.Format($"{pathStringXMLFolder}\\{file.FileName}");

            file.SaveAs(pathSaveXMLFile);
        }

    }
}
