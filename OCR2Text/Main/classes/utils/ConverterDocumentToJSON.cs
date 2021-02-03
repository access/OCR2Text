/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      28.09.2020
/// ==========================================

using RequestRecognitionToolLib.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


namespace RequestRecognitionToolLib.Main.classes.utils
{
    class ConverterDocumentToJSON
    {
        public static string GetDocumentJSON(List<Page> documentPages)
        {
            string tmp = string.Empty;
            List<string[]> listJSON = new List<string[]>();
            foreach (var page in documentPages)
                foreach (string[] row in page)
                    listJSON.Add(row);

            return JsonConvert.SerializeObject(listJSON, Formatting.Indented);
        }
    }
}
