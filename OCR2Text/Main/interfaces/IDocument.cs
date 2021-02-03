/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      23.09.2020
/// ==========================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RequestRecognitionToolLib.Main.classes;

namespace RequestRecognitionToolLib.Main.Interfaces
{
    public interface IDocument : IEnumerable, IEnumerator
    {
        int CountOfPages { get; }
        List<Page> DocumentPages { get; }
        string DocumentJSON { get; }
    }
}
