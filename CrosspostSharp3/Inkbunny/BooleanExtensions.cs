using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3.Inkbunny {
    internal static class BooleanExtensions {
        public static string ToYesNo(this bool b) {
            return b ? "yes" : "no";
        }
    }
}
