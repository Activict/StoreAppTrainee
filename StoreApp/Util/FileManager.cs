using StoreApp.Enums;
using System.Web;

namespace StoreApp.Util
{
    public class FileManager
    {
        private HttpPostedFileBase File { get; set; }
        public string StatusMessage;
        public string Message;

        private IFileParser Parser { get; set; }

        public FileManager(HttpPostedFileBase file, RootNames type)
        {
            File = file;

            if (File != null && File.ContentLength > 0)
            {
                Parser = GetParser(type);
            }
        }

        public bool IsValidateFile()
        {
            if (File == null || File.ContentLength <= 0)
            {
                StatusMessage = StateMessage.danger.ToString();
                Message = "File empty!";
                return false;
            }

            if (Parser == null)
            {
                Message = "File isn't XML";
                StatusMessage = StateMessage.danger.ToString();
                return false;
            }

            return true;
        }

        public bool IsValidateRequirements()
        {
            if (Parser != null)
            {
                StatusMessage = Parser.StatusMessage;
                Message = Parser.Message;
                return Parser.IsValidateRequirements();
            }

            return false;
        }

        private IFileParser GetParser(RootNames type)
        {
            if (File.ContentType == "text/xml")
            {
                return new FileParserXML(File, type);
            }

            if (File.ContentType == "application/json")
            {
                return new FileParserJSON(File, type);
            }

            return null;
        }

        public void SaveData()
        {
            if (Parser != null)
            {
                Parser.Saver.Save();
                Message = Parser.Saver.Message;
            }
        }
        public void SaveFile()
        {
            if (Parser != null)
            {
                Parser.Save();
            }
        }
    }
}