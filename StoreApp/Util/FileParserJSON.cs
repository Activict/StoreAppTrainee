using StoreApp.Enums;
using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace StoreApp.Util
{
    public class FileParserJSON : IFileParser
    {
        private RootNames type;
        private string json;
        private HttpPostedFileBase file;
        public ISaver Saver { get; set; }
        public string Message { get; set; }
        public string StatusMessage { get; set; }

        public FileParserJSON(HttpPostedFileBase file, RootNames type)
        {
            this.file = file;
            this.type = type;

            StreamReader streamFile = new StreamReader(file.InputStream);
            json = streamFile.ReadToEnd();

            if (IsValidateRequirements())
            {
                Saver = InitializeSaver();
            }
        }

        public bool IsValidateRequirements()
        {
            foreach (var rootName in Enum.GetValues(typeof(RootNames)))
            {
                if (file.FileName.StartsWith(rootName.ToString()) && file.FileName.StartsWith(type.ToString()))
                {
                    return true;
                }
            }

            StatusMessage = StateMessage.danger.ToString();
            Message = "File JSON does not meet requirements";

            return false;
        }

        private ISaver InitializeSaver()
        {
            switch (type)
            {
                case RootNames.products:
                    return new ParserProduct(json).GetSaver();
                case RootNames.units:
                    return new ParserUnit(json).GetSaver();
                case RootNames.categories:
                    return new ParserCategory(json).GetSaver();
                case RootNames.brands:
                    return new ParserBrand(json).GetSaver();
                case RootNames.producers:
                    return new ParserProducer(json).GetSaver();
                default:
                    return null;
            }
        }

        public void Save()
        {
            var path = ConfigurationManager.AppSettings.Get("pathUploadJSON");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var pathSaveXMLFile = $"{path}\\{file.FileName}";

            file.SaveAs(pathSaveXMLFile);
        }
    }
}