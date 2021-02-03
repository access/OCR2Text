/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      23.09.2020
/// ==========================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RequestRecognitionToolLib.Main.Interfaces
{
    public interface IPage : IEnumerable, IEnumerator
    {
        List<String[]> PageRows { get; }
        int MaxColumnsCount { get; }
        int MinColumnsCount { get; }
        int MaxRows { get; }
    }
}
