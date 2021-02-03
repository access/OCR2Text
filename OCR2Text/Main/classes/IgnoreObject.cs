/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      13.10.2020
/// ==========================================
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestRecognitionToolLib.Main.classes
{
    /// IgnoreObject - for black list words in JSON
    /// [
    ///  {
    ///    "IsCaseSensitive": false,
    ///    "IsRegularExpression": false,
    ///    "SkipWholeLine": false,
    ///    "ItemValue": "Saaja:"
    ///  }
    /// ]
    public class IgnoreObject
    {
        public bool IsCaseSensitive = false;
        public bool IsRegularExpression = false;
        public bool SkipWholeLine = false;
        public string ItemValue;
    }

}
