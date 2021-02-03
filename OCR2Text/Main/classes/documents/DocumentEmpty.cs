/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      24.09.2020
/// ==========================================
using RequestRecognitionToolLib.Main.Interfaces;
using RequestRecognitionToolLib.Main.classes.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RequestRecognitionToolLib.Main.classes
{
    public class DocumentEmpty : IDocument, IEnumerable, IEnumerator
    {
        public int CountOfPages { get; } = 0;
        public List<Page> DocumentPages { get; } = new List<Page>();
        public DocumentEmpty(IDataFile dataFile)
        {
            string[] row = new string[2];
            row[0] = "file extension: [" + dataFile.fileInfo.Extension.ToUpper() + "]";
            row[1] = "error: " + dataFile.LastErrorMsg;
            List<string[]> pageRows = new List<string[]>();
            pageRows.Add(row);
            Page page = new Page(pageRows);
            DocumentPages.Add(page);
        }
        public object Current => DocumentPages.GetEnumerator().Current;
        public string DocumentJSON => ConverterDocumentToJSON.GetDocumentJSON(DocumentPages);
        public bool MoveNext() => DocumentPages.GetEnumerator().MoveNext();
        public void Reset() => GetEnumerator().Reset();
        public IEnumerator GetEnumerator() => DocumentPages.GetEnumerator();
    }
}
