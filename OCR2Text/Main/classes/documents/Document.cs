using RequestRecognitionToolLib.Main.classes;
using RequestRecognitionToolLib.Main.classes.utils;
using RequestRecognitionToolLib.Main.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OCR2Text.Main.classes.documents
{
    public abstract class Document : IDocument, IEnumerable, IEnumerator
    {
        public int CountOfPages { get; set; } = 0;
        public List<Page> DocumentPages { get; } = new List<Page>();
        public Document()
        {
        }
        public object Current => DocumentPages.GetEnumerator().Current;
        public string DocumentJSON => ConverterDocumentToJSON.GetDocumentJSON(DocumentPages);
        public bool MoveNext() => DocumentPages.GetEnumerator().MoveNext();
        public void Reset() => GetEnumerator().Reset();
        public IEnumerator GetEnumerator() => DocumentPages.GetEnumerator();

    }
}
