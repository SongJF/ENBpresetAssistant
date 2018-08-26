using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENBpresetAssistant.Components;
using ENBpresetAssistant.Data;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

namespace ENBpresetAssistant.Tools
{
    public class PresetHelper
    {
        private static string PresetsConfigPath = SettingsData.StoragePath + ID.Dir_PresetJson;

        /// <summary>
        /// 返回ENB预设信息
        /// </summary>
        /// <returns></returns>
        public static List<PresetData> GetPresetFromJson()
        {
            try
            {
                string JsonStr = JsonHelper.GetJsonFromFile(PresetsConfigPath);
                if (JsonStr == "") JsonStr = JsonHelper.GetJsonInString(null);
                var Presets = JsonConvert.DeserializeObject<List<PresetData>>(JsonStr);
                return Presets;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return null;
            }
        }

        /// <summary>
        /// 初始化预设的Json文件
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// 添加一条preset
        /// </summary>
        /// <param name="preset"></param>
        public static void AddPresetJson(PresetData preset)
        {
            var CurrentPresets = GetPresetFromJson();
            if (CurrentPresets == null) CurrentPresets = new List<PresetData>();
            CurrentPresets.Add(preset);
            SavePrests(CurrentPresets);
        }

        /// <summary>
        /// 保存preset
        /// </summary>
        /// <param name="presets"></param>
        public static void SavePrests(List<PresetData> presets)
        {
            var JsonStr = JsonConvert.SerializeObject(presets);
            JsonHelper.JsonSave(PresetsConfigPath, JsonStr);
        }
    }
}
