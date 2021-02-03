/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      12.10.2020
/// ==========================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections;

namespace RequestRecognitionToolLib.Main.classes
{
    public class BlackListDictionary : IEnumerable, IEnumerator
    {
        private string _dictionaryFromFileJSON = "";
        private static string _dictPath;

        public List<IgnoreObject> IgnoreObjects = new List<IgnoreObject>();

        public BlackListDictionary()
        {
            byte[] _memBytes;
            try
            {
                Task<byte[]> readFileTask = Task.Run(() => ReadFileAsync(_dictPath));
                readFileTask.Wait();
                _memBytes = readFileTask.Result;
                _dictionaryFromFileJSON = System.Text.Encoding.UTF8.GetString(_memBytes, 0, _memBytes.Length);
            }
            catch (Exception) { }
            DeserializeDictionary();
        }

        private void DeserializeDictionary()
        {
            List<IgnoreObject> tmp = new List<IgnoreObject>();
            try { tmp = JsonConvert.DeserializeObject<List<IgnoreObject>>(_dictionaryFromFileJSON); } catch (Exception) { }
            if (tmp != null)
                IgnoreObjects = tmp;
        }

        public void AddIgnoreWord(string IgnoreValue, bool IsCaseSensitive = false, bool IsRegularExpression = false, bool SkipWholeLine = false)
        {
            bool hasDuplicates = false;
            foreach (var item in IgnoreObjects)
            {
                if (IsCaseSensitive && item.ItemValue == IgnoreValue)
                    hasDuplicates = true;
                else if (!IsCaseSensitive && item.ItemValue.ToUpper() == IgnoreValue.ToUpper())
                    hasDuplicates = true;
            }

            if (!hasDuplicates)
            {
                IgnoreObject blackWord = new IgnoreObject()
                {
                    ItemValue = IgnoreValue,
                    IsCaseSensitive = IsCaseSensitive,
                    IsRegularExpression = IsRegularExpression,
                    SkipWholeLine = SkipWholeLine
                };
                IgnoreObjects.Add(blackWord);
                string newJSON = JsonConvert.SerializeObject(IgnoreObjects, Formatting.Indented);
                WriteFile(_dictPath, newJSON);
            }
        }

        public void DeleteFilterWord(string DeleteValue)
        {
            IgnoreObject tmp = null;
            foreach (var item in IgnoreObjects)
            {
                if (item.ItemValue == DeleteValue)
                {
                    tmp = item;
                    break;
                }
            }
            if (tmp != null)
                IgnoreObjects.Remove(tmp);

            string newJSON = JsonConvert.SerializeObject(IgnoreObjects, Formatting.Indented);
            WriteFile(_dictPath, newJSON);
        }

        public static void Initialize(string pathToJDictionaryJSON)
        {
            _dictPath = pathToJDictionaryJSON;
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

        public void WriteFile(string path, string data)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length, true))
            {
                fs.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public List<string> GetFilterBlackList()
        {
            List<string> tmp = new List<string>();
            IgnoreObjects.ForEach(el => tmp.Add(el.ItemValue));
            return tmp;
        }

        public object Current => IgnoreObjects.GetEnumerator().Current;
        public bool MoveNext() => IgnoreObjects.GetEnumerator().MoveNext();
        public void Reset() => GetEnumerator().Reset();
        public IEnumerator GetEnumerator() => IgnoreObjects.GetEnumerator();

    }
}
