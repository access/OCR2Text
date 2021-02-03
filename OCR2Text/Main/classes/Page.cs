/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      23.09.2020
/// ==========================================
using RequestRecognitionToolLib.Main.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace RequestRecognitionToolLib.Main.classes
{
    public class Page : IPage
    {
        public List<string[]> PageRows { get; }
        public int MaxColumnsCount { get; } = 0;
        public int MaxRows { get; } = 0;
        public int MinColumnsCount { get; } = Int32.MaxValue;

        public Page(List<string[]> rows)
        {
            foreach (var row in rows)
            {
                MaxColumnsCount = row.Length > MaxColumnsCount ? row.Length : MaxColumnsCount;
                MinColumnsCount = row.Length < MinColumnsCount ? row.Length : MinColumnsCount;
                MaxRows++;
            }
            PageRows = rows;
        }

        public object Current => PageRows.GetEnumerator().Current;
        public IEnumerator GetEnumerator() => PageRows.GetEnumerator();
        public bool MoveNext() => PageRows.GetEnumerator().MoveNext();
        public void Reset() => GetEnumerator().Reset();
    }
}
