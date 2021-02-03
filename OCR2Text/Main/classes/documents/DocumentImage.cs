/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      01.10.2020
/// ==========================================
using RequestRecognitionToolLib.Main.classes.utils;
using RequestRecognitionToolLib.Main.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace RequestRecognitionToolLib.Main.classes
{
    internal class DocumentImage : IDocument, IEnumerable, IEnumerator
    {
        public DocumentImage(IDataFile dataFile)
        {
            ImageReader imageReader = new ImageReader(dataFile.GetBytes());
            Page page = new Page(imageReader.GetLines());
            DocumentPages.Add(page);
        }

        public object Current => DocumentPages.GetEnumerator().Current;
        public string DocumentJSON => ConverterDocumentToJSON.GetDocumentJSON(DocumentPages);
        public List<Page> DocumentPages { get; } = new List<Page>();
        public int CountOfPages => DocumentPages.Count;
        public bool MoveNext() => DocumentPages.GetEnumerator().MoveNext();
        public void Reset() => GetEnumerator().Reset();
        public IEnumerator GetEnumerator() => DocumentPages.GetEnumerator();
    }
}