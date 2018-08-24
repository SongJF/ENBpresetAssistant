using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ENBpresetAssistant.Data;
using Newtonsoft.Json;

namespace ENBpresetAssistant.Tools
{
    class CoreHelper
    {
        private static string CoresConfigPath = SettingsData.StoragePath + ID.Dir_CoreJson;

        public static List<CoreData> GetCoresFromJson()
        {
            try
            {
                string JsonStr = JsonHelper.GetJsonFromFile(CoresConfigPath);
                if (JsonStr == "") JsonStr = JsonHelper.GetJsonInString(null);
                var Presets = JsonConvert.DeserializeObject<List<CoreData>>(JsonStr);
                return Presets;
            }
            catch(Exception e)
            {
                Console.Write(e);
                return null;
            }
        }
    }
}
