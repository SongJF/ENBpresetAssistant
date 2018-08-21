using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ENBpresetAssistant.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ENBpresetAssistant.Tools
{
    public class PresetHelper
    {
        private static string PresetsConfigPath = SettingsData.StoragePath + "\\Presets.Json";

        /// <summary>
        /// 返回ENB预设信息
        /// </summary>
        /// <returns></returns>
        public static List<PresetData> GetPresetJson()
        {
            try
            {
                string JsonStr = JsonHelper.GetJsonFromFile(PresetsConfigPath);
                if (JsonStr == "") JsonStr = JsonHelper.GetJsonInString(null);
                var Presets = JsonConvert.DeserializeObject<List<PresetData>>(JsonStr);
                return Presets;
            }
            catch(Exception e)
            {
                Console.Write(e);
                return null;
            }
        }

        public static bool InitPresetJson()
        {
            try
            {
                var JsonStr = JsonHelper.GetJsonInString(null);
                JsonHelper.JsonSave(PresetsConfigPath, JsonStr);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
