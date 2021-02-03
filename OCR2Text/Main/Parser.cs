/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      21.09.2020
/// ==========================================
using RequestRecognitionToolLib.Main.classes;
using RequestRecognitionToolLib.Main.classes.utils;
using RequestRecognitionToolLib.Main.classes.utils.filter;
using RequestRecognitionToolLib.Main.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RequestRecognitionToolLib.Main
{
    public class ParserConfig
    {
        /// <summary>
        /// Tesseract OCR Library execution path, def. Windows: %programfiles%\Tesseract-OCR
        /// https://github.com/tesseract-ocr/tesseract
        /// License: GPL
        /// </summary>
        public string TesseractExecuteDir = @"%programfiles%\Tesseract-OCR";

        /// <summary>
        /// Tesseract OCR Library exetuble application filename with extension, like: tesseract.exe
        /// </summary>
        public string TesseractExecutableFile = "tesseract.exe";

        /// <summary>
        /// Text language for recognizer, def. "eng"
        /// </summary>
        public string Language = "eng";

        /// <summary>
        /// [Optional]
        /// Tesseract OCR tessdata path, def. %programfiles%\Tesseract-OCR\tessdata
        /// </summary>
        public string TessDataDir = @"%programfiles%\Tesseract-OCR\tessdata";

        /// <summary>
        /// Path to assemblies - Magick.Native-Q16-x64.dll, pdfium.dll, def. ~\runtimes\win-x64\native
        /// https://github.com/dlemstra/Magick.NET
        /// License: GPL
        /// </summary>
        public string NativeLibraryDirectory = "";

        /// <summary>
        /// Ghostscript  Library Path, with assemblies - gsdll64.dll, gswin64.exe
        /// https://www.ghostscript.com/download/gsdnld.html
        /// License: GNU AGPL
        /// </summary>
        public string GhostscriptDirectory = "";

        /// <summary>
        /// Path to filter words database (JSON)
        /// </summary>
        public string BlackListDictionaryPath = Path.Combine(Parser.AssemblyDirectory(), "blackListDictionary.json");

        /// <summary>
        /// How many columns in percentage of max columns to skip document lines (rows)
        /// </summary>
        public int CleanPercentageOfColumnsCount = 48;
    }

    public class Parser
    {
        private ParserConfig _config;
        private DocumentSelector _documentSelector = new DocumentSelector();
        private BlackListDictionary _filterDictionary;
        private Cleaner _cleaner;
        private IDocument _document;
        private IDataFile _dataFile;

        public IDocument GetDocument() => _document;
        public IDocument GetCleanDocument() => _cleaner.GetCleanDocument();
        public List<string> GetFilterBlackListWords() => _filterDictionary.GetFilterBlackList();
        public List<IgnoreObject> GetIgnoreObjects() => _filterDictionary.IgnoreObjects;
        public string DocumentJSON => _document.DocumentJSON;
        public string CleanDocumentJSON => _cleaner.GetCleanDocument().DocumentJSON;

        public Parser(ParserConfig config)
        {
            _config = config;
            Initialize();
        }

        public Parser(IDataFile dataFile, ParserConfig parserConfiguration)
        {
            _dataFile = dataFile;
            _config = parserConfiguration;
            Initialize();
        }

        public IEnumerable DocumentPages()
        {
            foreach (Page page in _document)
                yield return page;
        }

        public async Task GenerateDocumentAsync()
        {
            await Task.Run(() =>
            {
                _document = _documentSelector.GetDocumentByFileType(_dataFile);
                _filterDictionary = new BlackListDictionary();
                _cleaner = new Cleaner(_document, _filterDictionary);
            });
        }

        public async Task AddFilterWordAsync(string IgnoreValue, bool IsCaseSensitive = false, bool IsRegularExpression = false, bool SkipWholeLine = false)
        {
            await Task.Run(() => _filterDictionary.AddIgnoreWord(IgnoreValue, IsCaseSensitive, IsRegularExpression, SkipWholeLine));
        }

        public async Task DeleteFilterWordAsync(string DeleteValue) => await Task.Run(() => _filterDictionary.DeleteFilterWord(DeleteValue));

        public void DeleteFilterWord(string DeleteValue) => new BlackListDictionary().DeleteFilterWord(DeleteValue);

        public void AddFilterWord(string IgnoreValue, bool IsCaseSensitive, bool IsRegularExpression, bool SkipWholeLine)
        {
            new BlackListDictionary().AddIgnoreWord(IgnoreValue, IsCaseSensitive, IsRegularExpression, SkipWholeLine);
        }

        public static string AssemblyDirectory()
        {
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        private void Initialize()
        {
            ImageReader.Initialize(_config.TesseractExecuteDir, _config.TessDataDir, _config.Language, _config.TesseractExecutableFile);
            PdfToImageConverter.Initialize(_config.NativeLibraryDirectory, _config.GhostscriptDirectory);
            BlackListDictionary.Initialize(_config.BlackListDictionaryPath);
            Cleaner.Initialize(_config.CleanPercentageOfColumnsCount);
        }
    }
}
