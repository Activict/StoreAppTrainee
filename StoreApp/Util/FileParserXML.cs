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
        private RootNames type;
        private XmlElement xmlDocument;
        private HttpPostedFileBase file;
        public ISaver Saver { get; set; }
        public string Message { get; set; }
        public string StatusMessage { get; set; }

        public FileParserXML(HttpPostedFileBase file, RootNames type, IWebMapper mapper)
        {
            this.file = file;
            this.type = type;

            XmlDocument xmlFile = new XmlDocument();
            xmlFile.Load(this.file.InputStream);
            xmlDocument = xmlFile.DocumentElement;

            if (IsValidateRequirements())
            {
                Saver = GetSaver(mapper);
            }
        }

        public bool IsValidateRequirements()
        {
            if (Enum.IsDefined(typeof(RootNames), xmlDocument.Name) && xmlDocument.Name.Contains(type.ToString()))
            {
                return true;
            }

            StatusMessage = StateMessage.danger.ToString();
            Message = "File XML does not meet requirements";

            return false;
        }

        private ISaver GetSaver(IWebMapper mapper)
        {
            switch (type)
            {
                case RootNames.products:
                    return new ParserProduct(xmlDocument, mapper).GetSaver();
                case RootNames.units:
                    return new ParserUnit(xmlDocument, mapper).GetSaver();
                case RootNames.categories:
                    return new ParserCategory(xmlDocument, mapper).GetSaver();
                case RootNames.brands:
                    return new ParserBrand(xmlDocument, mapper).GetSaver();
                case RootNames.producers:
                    return new ParserProducer(xmlDocument, mapper).GetSaver();
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

            var pathSaveXMLFile = $"{path}\\{file.FileName}";

            file.SaveAs(pathSaveXMLFile);
        }
    }
}