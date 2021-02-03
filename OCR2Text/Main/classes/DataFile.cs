/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      21.09.2020
/// ==========================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RequestRecognitionToolLib.Main
{
    /// <summary>
    /// Class <c>DataFile</c> models data structure like "container" of IDataFile interface.
    /// </summary>

    public class DataFile : IDataFile
    {
        private readonly byte[] _memBytes;

        public bool HasLoadError { get; } = true;
        public string LastErrorMsg { get; } = string.Empty;
        public FileInfo fileInfo { get; }
        public byte[] GetBytes() => _memBytes;
        public MemoryStream GetMemoryStream() => new MemoryStream(_memBytes);

        public DataFile(string sourceFilePath)
        {
            fileInfo = new FileInfo(sourceFilePath);
            // read bytes into -> byte[] memBytes
            try
            {
                Task<byte[]> readFileTask = Task.Run(() => ReadFileAsync(sourceFilePath));
                readFileTask.Wait();
                _memBytes = readFileTask.Result;
                HasLoadError = false;
            }
            catch (Exception e) { LastErrorMsg = e.Message; }
        }

        private async Task<byte[]> ReadFileAsync(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var result = new byte[fileStream.Length];
                await fileStream.ReadAsync(result, 0, (int)fileStream.Length);
                return result;
            }
        }
    }
}
