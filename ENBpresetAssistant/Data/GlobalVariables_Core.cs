using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBpresetAssistant.Data
{
    public class GlobalVariables_Core
    {
        public static string ZipName { get; set; }

        public static string CoreVersion { get; set; }

        public static string RouterInTemp { get; set; }

        public static bool isCompelete { get; set; }

        public static void Init_Variables()
        {
            ZipName = null;
            CoreVersion = null;
            RouterInTemp = null;
            isCompelete = false;
        }
    }
}
