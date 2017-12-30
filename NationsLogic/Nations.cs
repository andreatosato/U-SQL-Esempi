using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NationsLogic
{
    public static class Nations
    {
        private static IEnumerable<string> EuroNations = new[] { "Italia", "Slovacchia", "Albania", "Francia", "Liechtenstein" };
        public static bool IsEuropean(string nation)
        {
            if (EuroNations.Contains(nation))
                return true;
            return false;
        }
    }
}