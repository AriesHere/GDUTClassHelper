using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GDUTClassHelper.Core.Common.Helper
{
    public static class StringHelper
    {
        /// <summary>Quotes must be paired. Escaped quotes are not handled.</summary>
        public static List<string> ExtractQuotedStrings(string source)
        {
            List<string> result = [];
            int prev = 0;
            bool flag = false;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '"')
                {
                    if (flag)
                    {
                        result.Add(source[(prev + 1)..(i)]);
                        flag = false;
                    }
                    else
                    {
                        prev = i;
                        flag = true;
                    }
                }
            }
            return result;
        }

        public static List<int> SplitByCharacterCount(string source, int c)
        {
            List<int> result = [];
            for (int i = 0; i < source.Length; i += c)
            {
                int end = i + c - 1;
                if (end < source.Length)
                {
                    result.Add(int.Parse(source[i..(end + 1)]));
                }
            }
            return result;
        }
    }
}
