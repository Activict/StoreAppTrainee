namespace StoreApp.Util
{
    public interface ISaver
    {
        string Message { get; }

        void Save();
    }
}
