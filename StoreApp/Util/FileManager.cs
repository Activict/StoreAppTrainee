using StoreApp.Enums;
using System.Web;

namespace StoreApp.Util
{
    public class FileManager
    {
        private HttpPostedFileBase file;
        private IFileParser parser;
        public string StatusMessage { get; private set; }
        public string Message { get; private set; }

        public FileManager(HttpPostedFileBase file, RootNames type, IWebMapper mapper)
        {
            this.file = file;

            if (this.file != null && this.file.ContentLength > 0)
            {
                parser = GetParser(type, mapper);
            }
        }

        public bool IsValidateFile()
        {
            if (file == null || file.ContentLength <= 0)
            {
                StatusMessage = StateMessage.danger.ToString();
                Message = "File empty!";
                return false;
            }

            if (parser == null)
            {
                Message = "File isn't XML";
                StatusMessage = StateMessage.danger.ToString();
                return false;
            }

            return true;
        }

        public bool IsValidateRequirements()
        {
            if (parser != null)
            {
                StatusMessage = parser.StatusMessage;
                Message = parser.Message;
                return parser.IsValidateRequirements();
            }

            return false;
        }

        private IFileParser GetParser(RootNames type, IWebMapper mapper)
        {
            if (file.ContentType == "text/xml")
            {
                return new FileParserXML(file, type, mapper);
            }

            if (file.ContentType == "application/json")
            {
                return new FileParserJSON(file, type, mapper);
            }

            return null;
        }

        public void SaveData()
        {
            if (parser != null)
            {
                parser.Saver.Save();
                Message = parser.Saver.Message;
            }
        }
        public void SaveFile()
        {
            if (parser != null)
            {
                parser.Save();
            }
        }
    }
}