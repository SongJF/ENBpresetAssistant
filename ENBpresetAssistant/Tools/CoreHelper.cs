﻿using System;
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

        /// <summary>
        /// 读取所有Core信息
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 保存core
        /// </summary>
        /// <param name="presets"></param>
        public static void SavePrests(List<CoreData> presets)
        {
            var JsonStr = JsonConvert.SerializeObject(presets);
            JsonHelper.JsonSave(CoresConfigPath, JsonStr);
        }
    }
}
