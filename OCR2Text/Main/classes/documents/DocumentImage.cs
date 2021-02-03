/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      01.10.2020
/// ==========================================
using OCR2Text.Main.classes.documents;
using RequestRecognitionToolLib.Main.classes.utils;
using RequestRecognitionToolLib.Main.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace RequestRecognitionToolLib.Main.classes
{
    internal class DocumentImage : Document
    {
        public DocumentImage(IDataFile dataFile)
        {
            ImageReader imageReader = new ImageReader(dataFile.GetBytes());
            Page page = new Page(imageReader.GetLines());
            DocumentPages.Add(page);
        }
    }
}