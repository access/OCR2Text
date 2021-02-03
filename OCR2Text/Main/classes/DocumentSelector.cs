/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      24.09.2020
/// ==========================================
using OCR2Text.Main.classes.documents;
using RequestRecognitionToolLib.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestRecognitionToolLib.Main.classes
{
    public class DocumentSelector
    {
        public Document GetDocumentByFileType(IDataFile dataFile)
        {
            if (!dataFile.HasLoadError)
            {
                switch (dataFile.fileInfo.Extension.ToUpper())
                {
                    case ".XLSX":
                        return new DocumentXLSX(dataFile);
                    case ".XLS":
                        return new DocumentXLS(dataFile);
                    case ".PDF":
                        return new DocumentPDF(dataFile);
                    case ".PNG":
                    case ".JPG":
                    case ".JPEG":
                    case ".GIF":
                        return new DocumentImage(dataFile);
                }
            }
            return new DocumentEmpty(dataFile);
        }
    }
}
