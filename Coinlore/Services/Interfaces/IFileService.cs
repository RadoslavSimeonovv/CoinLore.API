namespace Dynamo_Coinlore.Services.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<string>> ReadCoinsFile();

        Task WriteInLogFile(string path, string log);
    }
}
