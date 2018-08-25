using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBpresetAssistant.Data
{
    public class PresetData
    {
        public string PresetName { get; set; }
        public string Core { get; set; }
        public DateTime InstallTime { get; set; }
        public bool isRunning { get; set; }
    }
}
