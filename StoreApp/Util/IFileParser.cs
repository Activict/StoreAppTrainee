namespace StoreApp.Util
{
    public interface IFileParser
    {
        string Message { get; set; }
        string StatusMessage { get; set; }
        ISaver Saver { get; set; }

        void Save();
        bool IsValidateRequirements();
    }
}