﻿using System;
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
    }
}
