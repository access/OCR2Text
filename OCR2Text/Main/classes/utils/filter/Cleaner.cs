/// ==========================================
///  Title:     Recognizer for patterns from PDF, Image, Excel, etc. file types;
///  Author:    Jevgeni Kostenko
///  Date:      12.10.2020
/// ==========================================
using RequestRecognitionToolLib.Main.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RequestRecognitionToolLib.Main.classes.documents;
using System.Text.RegularExpressions;
using System.Linq;
using System.Diagnostics;

namespace RequestRecognitionToolLib.Main.classes.utils.filter
{
    public class Cleaner
    {
        private IDocument _document;
        private BlackListDictionary _dictionary;
        private static int _cleanPercentage = 50;

        public IDocument GetCleanDocument() => _document;

        public Cleaner(IDocument document, BlackListDictionary blackListDictionary)
        {
            _document = document;
            _dictionary = blackListDictionary;

            _document = GetCleanDocumentByBlackList(_document);
            _document = GetCleanDocumentByPercentageLengthFilter(_document, _cleanPercentage);
        }

        private IDocument GetCleanDocumentByPercentageLengthFilter(IDocument document, int percentage = 50)
        {
            Page cleanPage = document.DocumentPages.ElementAt(0);
            int maxColCount = cleanPage.MaxColumnsCount;
            double onePerc = (double)maxColCount / 100.0;
            List<string[]> pageRows = new List<string[]>();
            foreach (string[] row in cleanPage)
            {
                if (row.Length >= onePerc * percentage)
                {
                    pageRows.Add(row);
                }
            }
            cleanPage = new Page(pageRows);
            CleanDocument cleanDocument = new CleanDocument(cleanPage);
            return cleanDocument;
        }

        private IDocument GetCleanDocumentByBlackList(IDocument document)
        {
            Page newPage;// = new Page();
            List<string[]> newPageRows = new List<string[]>();

            foreach (Page page in document)
            {
                // filter row by SkipWholeLine - bool 
                string[] newFilteredRow = new string[0];
                foreach (string[] row in page)
                {
                    bool skipLine = false;
                    newFilteredRow = new string[0];
                    foreach (string word in row)
                    {
                        bool thisIsBlackWord = false;
                        // check each word for existing in the BlackListDictionary
                        foreach (IgnoreObject blackWord in _dictionary)
                        {
                            // if found blackword
                            if (word != null)
                                if (word.ToUpper() == blackWord.ItemValue.ToUpper() || blackWord.IsRegularExpression)
                                {
                                    // check first, if the word case NOT matched, check next 
                                    if (blackWord.IsCaseSensitive && word != blackWord.ItemValue)
                                        continue;
                                    // then check, if this NOT regular expression and in the properties of the blackword SkipWholeLine = true, then skip
                                    if (!blackWord.IsRegularExpression && blackWord.SkipWholeLine)
                                    {
                                        newFilteredRow = new string[0];
                                        skipLine = true;
                                        break;
                                    } // if regular expression matched, skip line
                                    else if (blackWord.IsRegularExpression && blackWord.SkipWholeLine)
                                    {
                                        Match m = Regex.Match(word, blackWord.ItemValue, RegexOptions.IgnoreCase);

                                        if (m.Success)
                                        {
                                            skipLine = true;
                                            break;
                                        }
                                    }
                                    else if (blackWord.IsRegularExpression && !blackWord.SkipWholeLine)
                                    {
                                        Match m = Regex.Match(word, blackWord.ItemValue, RegexOptions.IgnoreCase);
                                        if (m.Success)
                                        {
                                            thisIsBlackWord = true;
                                            break;
                                        }
                                    }
                                    else if (!blackWord.IsRegularExpression && !blackWord.SkipWholeLine)
                                    {
                                        // not regular, not skip line, matched in upper case = blackword
                                        thisIsBlackWord = true;
                                        break;
                                    }
                                }
                        }
                        if (skipLine) break;
                        if (word != null && !thisIsBlackWord && !String.IsNullOrEmpty(word.Trim()))
                            newFilteredRow = addWordToNewRow(newFilteredRow, word);
                    }
                    if (newFilteredRow.Length > 0)
                        newPageRows.Add(newFilteredRow);
                }
            }
            newPage = new Page(newPageRows);
            CleanDocument cleanDocument = new CleanDocument(newPage);
            return cleanDocument;
        }

        private string[] addWordToNewRow(string[] newFilteredRow, string word)
        {
            string[] tmp = new string[newFilteredRow.Length + 1];
            Array.Copy(newFilteredRow, tmp, newFilteredRow.Length);
            tmp[tmp.Length - 1] = word;
            return tmp;
        }

        public static void Initialize(int cleanPercentage)
        {
            _cleanPercentage = cleanPercentage;
        }
    }

}
