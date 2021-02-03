/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      13.10.2020
/// ==========================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OCR2Text.Main.classes.documents;
using RequestRecognitionToolLib.Main.classes.utils;
using RequestRecognitionToolLib.Main.Interfaces;


namespace RequestRecognitionToolLib.Main.classes.documents
{
    public class CleanDocument : Document
    {
        public CleanDocument(Page cleanPage)
        {
            DocumentPages.Add(cleanPage);
            CountOfPages = 1;
        }
    }
}
