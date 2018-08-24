using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBpresetAssistant.Data
{
    public class GlobalVariables_Core
    {
        private static List<CoreData> _Cores;
        public static List<CoreData> Cores
        {
            get { return _Cores; }

            set { _Cores = value; }
        }
    }
}
