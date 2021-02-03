/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      23.09.2020
/// ==========================================
using System;
using System.Collections.Generic;
using System.Text;
using ExcelDataReader;
using System.IO;
using System.Collections;
using RequestRecognitionToolLib.Main.Interfaces;
using RequestRecognitionToolLib.Main.classes;
using RequestRecognitionToolLib.Main.classes.utils;
using OCR2Text.Main.classes.documents;

namespace RequestRecognitionToolLib.Main
{
    public class DocumentXLS : Document
    {
        public DocumentXLS(IDataFile dataFile)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            try
            {
                using (var reader = ExcelReaderFactory.CreateReader(new MemoryStream(dataFile.GetBytes())))
                {

                    CountOfPages = reader.ResultsCount;
                    // worksheets_count:  reader.ResultsCount;
                    // rows_count:        reader.RowCount;
                    // columns_count:     reader.FieldCount;
                    List<string[]> pageRows = new List<string[]>();
                    for (int i = 0; i < CountOfPages; i++)
                    {
                        pageRows = new List<string[]>();
                        while (reader.Read())                                   // for a each row on the page
                        {
                            string[] row = new string[reader.FieldCount];       // create tmp row with columns size
                            bool rowIsEmpty = true;
                            for (int c = 0; c < reader.FieldCount; c++)         // for each column in  the row
                            {
                                if (!reader.IsDBNull(c))
                                { // GetFieldType() : double, int, bool, DateTime, TimeSpan, string, or null if there is no value.
                                    row[c] = reader.GetValue(c).ToString();     // add the value of column to row array
                                    if (row[c].Trim() != string.Empty)
                                        rowIsEmpty = false;
                                }
                            }
                            if (!rowIsEmpty)
                                pageRows.Add(row);
                        }
                        Page page = new Page(pageRows);
                        DocumentPages.Add(page);
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
