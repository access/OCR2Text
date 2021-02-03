/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      23.09.2020
/// ==========================================
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Xml;
using System.Drawing;
using OfficeOpenXml.Style;
using RequestRecognitionToolLib.Main.Interfaces;
using RequestRecognitionToolLib.Main.classes;
using RequestRecognitionToolLib.Main.classes.utils;
using System.Collections;
using ExcelDataReader;
using OCR2Text.Main.classes.documents;

namespace RequestRecognitionToolLib.Main
{
    public class DocumentXLSX : Document
    {

        public DocumentXLSX(IDataFile dataFile)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage(new MemoryStream(dataFile.GetBytes())))
                {
                    int idx = 0;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[idx] == null ? package.Workbook.Worksheets[idx = 1] : package.Workbook.Worksheets[idx];
                    do
                    {
                        if (worksheet != null)
                        {   // have bugfix on some empty (hidden) worksheets, Dimension = null, check next
                            if (worksheet.Dimension == null)
                                goto tryNextWorksheet;
                            int colCount = worksheet.Dimension.End.Column;  //get column count
                            int rowCount = worksheet.Dimension.End.Row;     //get row count
                            string[] _row = new string[colCount];
                            List<string[]> pageRows = new List<string[]>();
                            for (int row = 1; row <= rowCount; row++)
                            {
                                bool rowIsEmpty = true;
                                _row = new string[colCount];
                                string tmpVal = "";
                                //pageRows = new List<string[]>();
                                for (int col = 1; col <= colCount; col++)
                                {
                                    if (worksheet.Cells[row, col].Value != null)
                                        if (!String.IsNullOrEmpty(tmpVal = worksheet.Cells[row, col].Value.ToString().Trim()))
                                            _row[col - 1] = tmpVal;
                                    string colVal = _row[col - 1];
                                    if (!String.IsNullOrEmpty(colVal))
                                        rowIsEmpty = false;
                                }
                                if (!rowIsEmpty)
                                    pageRows.Add(_row);
                            }
                            Page page = new Page(pageRows);
                            DocumentPages.Add(page);
                            CountOfPages++;
                        tryNextWorksheet:
                            try
                            {
                                worksheet = package.Workbook.Worksheets[idx + 1] == null ? null : package.Workbook.Worksheets[++idx];
                            }
                            catch (Exception) { break; }
                        }
                        else
                            break;
                    } while (true);
                }
            }
            catch (Exception) { }
        }
    }
}
