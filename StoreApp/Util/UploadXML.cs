using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;

namespace StoreApp.Util
{
    public abstract class UploadXML
    {
        protected HttpPostedFileBase File { get; set; }
        protected WebMapper webMapper { get; set; }
        protected XmlElement Root { get; set; }
        private XmlDocument xmlFile { get; set; }

        public int countUpload;
        public int countNotUpload;

        public UploadXML(HttpPostedFileBase file)
        {
            File = file;
            webMapper = new WebMapper();
        }

        public bool IsValidateFile()
        {
            if (File == null || File.ContentLength <= 0 || File.ContentType != "text/xml")
            {
                return false;
            }

            InitializeRoot();

            return true;
        }

        protected void InitializeRoot()
        {
            if (xmlFile == null)
            {
                xmlFile = new XmlDocument();
                xmlFile.Load(File.InputStream);
                
                Root = xmlFile.DocumentElement;
            }
        }

        abstract public bool IsValidateRoot();

        abstract public void SaveToDB();

        public void SaveXML()
        {
            var path = ConfigurationManager.AppSettings.Get("pathUploadXML");
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var pathSaveXMLFile = string.Format($"{path}\\{File.FileName}");

            File.SaveAs(pathSaveXMLFile);
        }

    }

}