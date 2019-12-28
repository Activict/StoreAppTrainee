using System;
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

            return true;
        }

        protected void InitializeRoot()
        {
            xmlFile = new XmlDocument();

            xmlFile.Load(File.InputStream);

            Root = xmlFile.DocumentElement;
        }

        abstract public bool IsValidateRoot();

        abstract public void SaveToDB();

        public void SaveXML()
        {
            var pathStringContent = new DirectoryInfo(string.Format($"{AppDomain.CurrentDomain.BaseDirectory}Content\\"));
            var pathStringContentXML = Path.Combine(pathStringContent.ToString(), "XMLUploads");

            if (!Directory.Exists(pathStringContentXML))
            {
                Directory.CreateDirectory(pathStringContentXML);
            }

            var pathSaveXMLFile = string.Format($"{pathStringContentXML}\\{File.FileName}");

            File.SaveAs(pathSaveXMLFile);
        }

    }

}