﻿/*************************************************************************
 *  Copyright © 2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Internationalizer.cs
 *  Description  :  Internationalizer.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  4/12/2019
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.DesignPattern;
using MGS.Logger;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MGS.Internation
{
    /// <summary>
    /// Internationalizer.
    /// </summary>
    public sealed class Internationalizer : Singleton<Internationalizer>
    {
        #region Field and Property
        /// <summary>
        /// Separator of paragraph key and value.
        /// </summary>
        public static readonly char[] SEPARATOR = new char[] { '=' };

        /// <summary>
        /// Current language name.
        /// </summary>
        public string Current
        {
            set
            {
                if (languages.ContainsKey(value))
                {
                    current = value;
                }
                else
                {
                    LogUtility.LogError("Set current language error: The language {0} is not Initialized.", value);
                }
            }
            get { return current; }
        }

        /// <summary>
        /// Current language name.
        /// </summary>
        private string current = null;

        /// <summary>
        /// Languages paragraphs dictionary.
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> languages = null;
        #endregion

        #region Private Method
        /// <summary>
        /// Constructor.
        /// </summary>
        private Internationalizer()
        {
            languages = new Dictionary<string, Dictionary<string, string>>();
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Deserialize language paragraphs from local file.
        /// </summary>
        /// <param name="language">Name of language.</param>
        /// <param name="languageFile">File path of language content.</param>
        /// <param name="encoding">Encoding of file content.</param>
        /// <returns>Deserialize succeed?</returns>
        public bool Deserialize(string language, string languageFile, Encoding encoding)
        {
            if (string.IsNullOrEmpty(language))
            {
                LogUtility.LogError("Deserialize language error: The language name is null or empty.");
                return false;
            }

            if (!File.Exists(languageFile))
            {
                LogUtility.LogError("Deserialize language error: Can not find the language file at path {0}", languageFile);
                return false;
            }

            var fileLines = File.ReadAllLines(languageFile, encoding);
            if (fileLines == null || fileLines.Length == 0)
            {
                LogUtility.LogError("Deserialize language error: Can not read any content in the language file at path {0}", languageFile);
                return false;
            }

            return Deserialize(language, fileLines);
        }

        /// <summary>
        /// Deserialize language paragraphs from paragraph lines.
        /// </summary>
        /// <param name="language">Name of language.</param>
        /// <param name="paragraphLines">Paragraph lines of language.</param>
        /// <returns>Deserialize succeed?</returns>
        public bool Deserialize(string language, string[] paragraphLines)
        {
            if (string.IsNullOrEmpty(language))
            {
                LogUtility.LogError("Deserialize language error: The language name is null or empty.");
                return false;
            }

            if (paragraphLines == null)
            {
                LogUtility.LogError("Deserialize language error: The paragraph lines is null.");
                return false;
            }

            if (languages.ContainsKey(language))
            {
                //Clear origin language content.
                languages[language].Clear();
            }
            else
            {
                languages.Add(language, new Dictionary<string, string>());
            }

            foreach (var line in paragraphLines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    //Ignore empty line.
                    continue;
                }

                var contents = line.Split(SEPARATOR, 2, System.StringSplitOptions.RemoveEmptyEntries);
                if (contents.Length < 2)
                {
                    //Ignore invalid line.
                    continue;
                }
                languages[language].Add(contents[0], contents[1]);
            }

            return true;
        }

        /// <summary>
        /// Get all deserialized languages.
        /// </summary>
        /// <returns>All deserialized languages.</returns>
        public string[] GetLanguages()
        {
            var languageArray = new string[languages.Keys.Count];
            languages.Keys.CopyTo(languageArray, 0);
            return languageArray;
        }

        /// <summary>
        /// Get paragraphs of target language.
        /// </summary>
        /// <param name="language">Name of language.</param>
        /// <returns>Paragraphs of target language.</returns>
        public Dictionary<string, string> GetParagraphs(string language)
        {
            if (!languages.ContainsKey(language))
            {
                LogUtility.LogError("Get GetParagraphs error: The language {0} is not Initialized.", language);
                return null;
            }

            return languages[language];
        }

        /// <summary>
        /// Get a paragraph text of key in current language.
        /// </summary>
        /// <param name="key">Key of paragraph text.</param>
        /// <returns>A paragraph text of key in current language.</returns>
        public string GetParagraph(string key)
        {
            if (string.IsNullOrEmpty(current))
            {
                LogUtility.LogError("Get paragraph error: The current language name is not set.");
                return null;
            }

            return GetParagraph(current, key);
        }

        /// <summary>
        /// Get a paragraph text of key in language.
        /// </summary>
        /// <param name="language">Name of language.</param>
        /// <param name="key">Key of paragraph text.</param>
        /// <returns>A paragraph text of key in language.</returns>
        public string GetParagraph(string language, string key)
        {
            if (!languages.ContainsKey(language))
            {
                LogUtility.LogError("Get paragraph error: The language {0} is not Initialized.", language);
                return null;
            }

            if (!languages[language].ContainsKey(key))
            {
                LogUtility.LogError("Get paragraph error: The key {0} can not find in the content of language {1}.", key, language);
                return null;
            }

            return languages[language][key];
        }

        /// <summary>
        /// Clear paragraphs of language.
        /// </summary>
        /// <param name="language">Name of language.</param>
        public void ClearLanguage(string language)
        {
            if (languages.ContainsKey(language))
            {
                languages.Remove(language);
            }

            if (language == current)
            {
                current = string.Empty;
            }
        }

        /// <summary>
        /// Clear paragraphs of languages.
        /// </summary>
        public void ClearLanguages()
        {
            languages.Clear();
            current = string.Empty;
        }
        #endregion
    }
}