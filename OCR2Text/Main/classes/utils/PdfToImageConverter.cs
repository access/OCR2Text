/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      28.09.2020
/// ==========================================
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ImageMagick;
using Docnet;
using ImageMagick.Configuration;
using System.Collections;

namespace RequestRecognitionToolLib.Main.classes.utils
{
    public class PdfToImageConverter : IEnumerable, IEnumerator
    {
        private List<byte[]> _imagesList { get; set; } = new List<byte[]>();
        private static string _nativeLibraryDirectory = "";
        private static string _GhostscriptDirectory = "";

        public PdfToImageConverter(IDataFile dataFile)
        {
            MagickNET.SetNativeLibraryDirectory(_nativeLibraryDirectory);
            MagickNET.SetGhostscriptDirectory(_GhostscriptDirectory);
            MagickNET.SetTempDirectory(Path.GetTempPath());
            MagickAnyCPU.CacheDirectory = Path.GetTempPath();

            var settings = new MagickReadSettings
            {
                // Settings the density to 300 dpi will create an image with a better quality
                Density = new Density(300)
            };

            var images = new MagickImageCollection();

            // Add all the pages of the pdf file to the collection
            images.Read(dataFile.GetBytes(), settings);

            MemoryStream ms = new MemoryStream();
            foreach (var p in images)
            {
                ms = new MemoryStream();
                p.Write(ms, MagickFormat.Png);
                _imagesList.Add(ms.ToArray());

                //File.WriteAllBytes(@"D:\tmp\" + c++ + "_" + dataFile.fileInfo.Name + ".png", ms.ToArray());
            }
            images.Clear();
        }

        public object Current => _imagesList.GetEnumerator().Current;
        public bool MoveNext() => _imagesList.GetEnumerator().MoveNext();
        public void Reset() => GetEnumerator().Reset();
        public IEnumerator GetEnumerator() => _imagesList.GetEnumerator();

        public static void Initialize(string SetNativeLibraryDirectory, string SetGhostscriptDirectory)
        {
            _nativeLibraryDirectory = SetNativeLibraryDirectory;
            _GhostscriptDirectory = SetGhostscriptDirectory;
        }
    }
}
