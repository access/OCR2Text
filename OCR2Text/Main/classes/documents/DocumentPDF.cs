/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      01.10.2020
/// ==========================================
using RequestRecognitionToolLib.Main.classes.utils;
using RequestRecognitionToolLib.Main.Interfaces;
using RequestRecognitionToolLib.Main.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImageMagick;
using Docnet;
using System.Threading.Tasks;
using System.Collections;

namespace RequestRecognitionToolLib.Main.classes
{
    public class DocumentPDF : IDocument, IEnumerable, IEnumerator
    {
        public List<Page> DocumentPages { get; } = new List<Page>();
        public int CountOfPages => DocumentPages.Count;
        public DocumentPDF(IDataFile dataFile)
        {
            PdfToImageConverter imagesFromPDF = new PdfToImageConverter(dataFile);
            
            foreach (byte[] imagePage in imagesFromPDF)
            {
                ImageReader imgReader = new ImageReader(imagePage);
                Page page = new Page(imgReader.GetLines());
                DocumentPages.Add(page);
            }
        }
        public object Current => DocumentPages.GetEnumerator().Current;
        public string DocumentJSON => ConverterDocumentToJSON.GetDocumentJSON(DocumentPages);
        public bool MoveNext() => DocumentPages.GetEnumerator().MoveNext();
        public void Reset() => GetEnumerator().Reset();
        public IEnumerator GetEnumerator() => DocumentPages.GetEnumerator();
    }
}
