/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      13.10.2020
/// ==========================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RequestRecognitionToolLib.Main.classes.utils;
using RequestRecognitionToolLib.Main.Interfaces;


namespace RequestRecognitionToolLib.Main.classes.documents
{
    public class CleanDocument : IDocument, IEnumerable, IEnumerator
    {
        public int CountOfPages { get; } = 0;
        public List<Page> DocumentPages { get; } = new List<Page>();
        public CleanDocument(Page cleanPage)
        {
            DocumentPages.Add(cleanPage);
            CountOfPages = 1;
        }
        public object Current => DocumentPages.GetEnumerator().Current;
        public string DocumentJSON => ConverterDocumentToJSON.GetDocumentJSON(DocumentPages);
        public bool MoveNext() => DocumentPages.GetEnumerator().MoveNext();
        public void Reset() => GetEnumerator().Reset();
        public IEnumerator GetEnumerator() => DocumentPages.GetEnumerator();
    }
}
