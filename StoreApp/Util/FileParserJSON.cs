using Newtonsoft.Json;
using StoreApp.BLL.DTO;
using StoreApp.Enums;
using StoreApp.Models.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;

namespace StoreApp.Util
{
    public class FileParserJSON : IFileParser
    {
        private RootNames Type { get; set; }
        private string Json { get; set; }
        private HttpPostedFileBase File { get; set; }
        public ISaver Saver { get; set; }
        public string Message { get; set; }
        public string StatusMessage { get; set; }

        public FileParserJSON(HttpPostedFileBase file, RootNames type)
        {
            File = file;
            Type = type;

            StreamReader streamFile = new StreamReader(file.InputStream);
            Json = streamFile.ReadToEnd();

            if (IsValidateRequirements())
            {
                Saver = InitializeSaver();
            }
        }

        public bool IsValidateRequirements()
        {
            foreach (var rootName in Enum.GetValues(typeof(RootNames)))
            {
                if (File.FileName.StartsWith(rootName.ToString()) && File.FileName.StartsWith(Type.ToString()))
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
            switch (Type)
            {
                case RootNames.products:
                    return new ParserProduct(Json).GetSaver();
                case RootNames.units:
                    return new ParserUnit(Json).GetSaver();
                case RootNames.categories:
                    return new ParserCategory(Json).GetSaver();
                case RootNames.brands:
                    return new ParserBrand(Json).GetSaver();
                case RootNames.producers:
                    return new ParserProducer(Json).GetSaver();
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

            var pathSaveXMLFile = $"{path}\\{File.FileName}";

            File.SaveAs(pathSaveXMLFile);
        }
    }
}