using Dynamo_Coinlore.Services.Interfaces;

namespace Dynamo_Coinlore.Services
{
    public class FileService : IFileService
    {
        /// <summary>
        /// Read from file where the initial portfolio is 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ReadCoinsFile()
        {
            var path = Path.Combine(Environment.CurrentDirectory, @"coinlore.txt");
            var fileLines = await File.ReadAllLinesAsync(path).ConfigureAwait(false);

            return fileLines;
        }

        /// <summary>
        /// Write operations in log file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task WriteInLogFile(string path, string log)
            => await File.AppendAllTextAsync(path, log).ConfigureAwait(false);
    }
}

