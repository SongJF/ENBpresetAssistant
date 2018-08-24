using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBpresetAssistant.Data
{
    class GlobalVariables_Preset
    {
        public static string ZipName { get; set; }

        public static string CoreVersion { get; set; }

        public static string RouterInTemp { get; set; }

        public static bool AddComplete { get; set; }

        public static void Init_Variables()
        {
            ZipName = null;
            CoreVersion = null;
            RouterInTemp = null;
            AddComplete = false;
        }
    }
}
