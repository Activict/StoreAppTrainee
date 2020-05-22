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

        public FileParserJSON(HttpPostedFileBase file, RootNames type, IWebMapper mapper)
        {
            this.file = file;
            this.type = type;

            StreamReader streamFile = new StreamReader(file.InputStream);
            json = streamFile.ReadToEnd();

            if (IsValidateRequirements())
            {
                Saver = InitializeSaver(mapper);
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

        private ISaver InitializeSaver(IWebMapper mapper)
        {
            switch (type)
            {
                case RootNames.products:
                    return new ParserProduct(json, mapper).GetSaver();
                case RootNames.units:
                    return new ParserUnit(json, mapper).GetSaver();
                case RootNames.categories:
                    return new ParserCategory(json, mapper).GetSaver();
                case RootNames.brands:
                    return new ParserBrand(json, mapper).GetSaver();
                case RootNames.producers:
                    return new ParserProducer(json, mapper).GetSaver();
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