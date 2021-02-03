/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      28.09.2020
/// ==========================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace RequestRecognitionToolLib.Main.classes.utils
{
    public class ImageReader
    {
        private static string _tesseractExeDirectory;
        private static string _tesseractExeFilePath;
        private static string _tessDataDir = null;
        private static string _language;
        private String plainTxt = string.Empty;
        private List<string[]> _rows = new List<string[]>();

        public List<string[]> GetLines() => _rows;
        public string getAllText() => plainTxt;

        public ImageReader() { }

        public ImageReader(byte[] imageBytes)
        {
            plainTxt = GetText(new MemoryStream(imageBytes));

            var textRows = Regex.Split(plainTxt, "\r\n|\r|\n");
            foreach (var row in textRows)
            {
                if (row.Trim() != string.Empty)
                    _rows.Add(Regex.Split(row, @"\s+"));
            }
        }

        public string GetText(Stream image)
        {
            var output = string.Empty;
            var tempInputFile = Path.GetTempFileName();
            var tempOutputFile = Path.GetTempFileName();

            try
            {
                WriteInputFile(image, tempInputFile);

                var info = new ProcessStartInfo
                {
                    FileName = _tesseractExeFilePath,
                    Arguments = $"\"{tempInputFile}\" \"{tempOutputFile}\" -l {_language} --psm 1", // -c paragraph_text_based=false",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                Process ps = new Process();
                using (ps = Process.Start(info))
                {
                    ps.WaitForExit();
                    var exitCode = ps.ExitCode;

                    if (exitCode == 0)
                    {
                        output = File.ReadAllText(tempOutputFile + ".txt");
                        File.Delete(tempOutputFile + ".txt");
                    }
                    else
                    {
                        var stderr = ps.StandardError.ReadToEnd();
                        throw new InvalidOperationException(stderr);
                    }
                    ps.Close();
                }
            }
            finally
            {
                File.Delete(tempInputFile);
                File.Delete(tempOutputFile);
            }
            return output;
        }

        private static void WriteInputFile(Stream inputStream, string tempInputFile)
        {
            using (var tempStream = File.OpenWrite(tempInputFile))
            {
                CopyStream(inputStream, tempStream);
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            if (input.CanSeek)
                input.Seek(0, SeekOrigin.Begin);
            input.CopyTo(output);
            input.Close();
        }

        public static void Initialize(string TesseractExecuteDir, string TessDataDir = "", string Language = "eng", string TesseractExecutableAppFile = "tesseract.exe")
        {
            _tesseractExeDirectory = TesseractExecuteDir;
            _tessDataDir = TessDataDir;
            _language = Language;

            _tesseractExeFilePath = Path.Combine(_tesseractExeDirectory, TesseractExecutableAppFile);
            if (String.IsNullOrEmpty(_tessDataDir))
                _tessDataDir = Path.Combine(_tesseractExeFilePath, "tessdata");
            Environment.SetEnvironmentVariable("TESSDATA_PREFIX", _tessDataDir);
        }

    }
}
