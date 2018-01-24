using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin
{
    public static class StringExtensions
    {
        public const char WORD_NEWLINE = '\r';

        public static string RemoveWordNewLines(this string str)
        {
            // TODO: OH god, this method is not very fast.... 
            while (str.Contains(WORD_NEWLINE))
            {
                str = str.Replace(""+WORD_NEWLINE, "");
            }
            return str;
        }

    }
}
