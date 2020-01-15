using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParseLibrary
{
    public static class StringParseLibrary
    {
        private static readonly String delimiter = " ";

        public static string[] GetWordsInString(String text)
        {
            return text.Trim().Split(StringParseLibrary.delimiter.ToCharArray()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
    }
}
