using StoreApp.Enums;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;

namespace StoreApp.Util
{
    public class FileParserXML : IFileParser
    {
        private RootNames Type;
        private XmlElement XmlDocument { get; set; }
        protected HttpPostedFileBase File { get; set; }
        public ISaver Saver { get; set; }
        public string Message { get; set; }
        public string StatusMessage { get; set; }

        public FileParserXML(HttpPostedFileBase file, RootNames type)
        {
            File = file;
            Type = type;

            XmlDocument xmlFile = new XmlDocument();
            xmlFile.Load(File.InputStream);
            XmlDocument = xmlFile.DocumentElement;

            if (IsValidateRequirements())
            {
                Saver = GetSaver();
            }
        }

        public bool IsValidateRequirements()
        {
            if (Enum.IsDefined(typeof(RootNames), XmlDocument.Name) && XmlDocument.Name.Contains(Type.ToString()))
            {
                return true;
            }

            StatusMessage = StateMessage.danger.ToString();
            Message = "File XML does not meet requirements";

            return false;
        }

        private ISaver GetSaver()
        {
            switch (Type)
            {
                case RootNames.products:
                    return new ParserProduct(XmlDocument).GetSaver();
                case RootNames.units:
                    return new ParserUnit(XmlDocument).GetSaver();
                case RootNames.categories:
                    return new ParserCategory(XmlDocument).GetSaver();
                case RootNames.brands:
                    return new ParserBrand(XmlDocument).GetSaver();
                case RootNames.producers:
                    return new ParserProducer(XmlDocument).GetSaver();
                default:
                    return null;
            }
        }

        public void Save()
        {
            var path = ConfigurationManager.AppSettings.Get("pathUploadXML");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var pathSaveXMLFile = $"{path}\\{File.FileName}";

            File.SaveAs(pathSaveXMLFile);
        }
    }
}