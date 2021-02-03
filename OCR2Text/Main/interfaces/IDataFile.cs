/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      21.09.2020
/// ==========================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RequestRecognitionToolLib.Main
{
    public interface IDataFile
    {
        /// <summary>
        /// Interface <c>IDataFile</c> models universal structure for defferent files, like: PDF,XLS,JPG,PNG, etc.
        /// </summary>

        MemoryStream GetMemoryStream();
        byte[] GetBytes();
        bool HasLoadError { get; }
        string LastErrorMsg { get; }
        FileInfo fileInfo { get; }
    }
}
