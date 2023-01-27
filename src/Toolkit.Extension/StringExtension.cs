using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class StringExtension
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp) =>
            (source.IndexOf(toCheck, comp) >= 0);

        public static List<string> CommonSplit(this string input)
        {
            if (input == null) return null;
            string data = input.Trim();
            if (data == null) return null;
            string[] separators = new string[] { ",", "，", ";", "；", "/", "\n", " " };
            var result = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return result.ToList();
        }
    }
}
