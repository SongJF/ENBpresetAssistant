using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ENBpresetAssistant.Data;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ENBpresetAssistant.Tools
{
    class SettingsHelper
    {
        /// <summary>
        /// 程序初始读取设置
        /// </summary>
        /// <returns></returns>
        public static bool ReadSettings()
        {
            string SettingsPath = Directory.GetCurrentDirectory() + "\\Setings.Json";
            String JsonString = GetJsonFromFile(SettingsPath);

            if(JsonString.Length==0)
            {
                InitGlobal();
                Save(SettingsPath);
                return true;
            }

            JObject Settings = (JObject)JsonConvert.DeserializeObject(JsonString);

            if (!SetGlobalSettings(Settings)) return false;
            return true;
        }

        /// <summary>
        /// 修改设置
        /// </summary>
        /// <param name="option">修改项</param>
        /// <param name="value">修改值</param>
        /// <returns></returns>
        public static bool ModifySettings(string option,string value)
        {
            try
            {
                string SettingsPath = Directory.GetCurrentDirectory() + "\\Setings.Json";
                SetGlobal(option, value);
                Save(SettingsPath);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 全局初始化
        /// </summary>jw.WritePropertyName("isDark");
        /// <param name="Settings">读取的设置</param>
        /// <returns></returns>
        private static bool SetGlobalSettings(JObject Settings)
        {
            try
            {
                foreach(var Setting in Settings)
                {
                    SetGlobal(Setting.Key.ToString(), Setting.Value.ToString());
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设定全局设置变量
        /// </summary>
        /// <param name="option">写入全局设置的项</param>
        /// <param name="value">写入的值</param>
        /// <returns></returns>
        private static bool SetGlobal(string option,string value)
        {
            switch (option)
            {
                case "isDark":
                    SettingsData.isDark= Convert.ToBoolean(value);
                    break;
                case "ThemeColor":
                    SettingsData.ThemeColor = value;
                    break;
                case "TESVPath":
                    SettingsData.TESVPath = value;
                    break;
                case "ENBCoresPath":
                    SettingsData.ENBCoresPath = value;
                    break;
                case "ENBPresetPath":
                    SettingsData.ENBPresetPath = value;
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 从本地读取Json文件
        /// </summary>
        /// <param name="SettingsPath">Json文件路径</param>
        /// <returns></returns>
        private static string GetJsonFromFile(string SettingsPath)
        {
            if (!File.Exists(SettingsPath))
            {
                var thisFile = File.Create(SettingsPath);
                thisFile.Close();
            }

            string JsonString = File.ReadAllText(SettingsPath);

            return JsonString;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="SettingsPath"></param>
        /// <returns></returns>
        private static bool Save(string SettingsPath)
        {
            try
            {
                File.WriteAllText(SettingsPath, GetJsonInString());
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 序列化Json
        /// </summary>
        /// <returns></returns>
        private static string GetJsonInString()
        {
            StringWriter JsonString = new StringWriter();
            JsonWriter jw=new JsonTextWriter(JsonString);

            jw.WriteStartObject();
            jw.WritePropertyName("isDark");
            jw.WriteValue(SettingsData.isDark);
            jw.WritePropertyName("ThemeColor");
            jw.WriteValue(SettingsData.ThemeColor);
            jw.WritePropertyName("TESVPath");
            jw.WriteValue(SettingsData.TESVPath);
            jw.WritePropertyName("ENBCoresPath");
            jw.WriteValue(SettingsData.ENBCoresPath);
            jw.WritePropertyName("ENBPresetPath");
            jw.WriteValue(SettingsData.ENBPresetPath);
            jw.WriteEndObject();
            jw.Flush();

            return JsonString.GetStringBuilder().ToString();
        }

        /// <summary>
        /// 初始化全局设置
        /// </summary>
        private static void InitGlobal()
        {
            SetGlobal("isDark", "false");
            SetGlobal("ThemeColor", "brown");
            SetGlobal("ENBCoresPath", Directory.GetCurrentDirectory() + "\\Cores");
            SetGlobal("ENBPresetPath", Directory.GetCurrentDirectory() + "\\Preset");
        }
    }
}
