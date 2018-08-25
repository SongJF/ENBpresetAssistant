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
            String JsonString = JsonHelper.GetJsonFromFile(SettingsPath);

            if(JsonString.Length<=2)
            {
                InitGlobal();
                return true;
            }

            JObject Settings = JsonHelper.TransStrToJObject(JsonString);

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
                if (!SetGlobal(option, value)) throw new ArgumentOutOfRangeException("Unvalid Setting Option");
                JsonHelper.JsonSave(SettingsPath,TransGlobalSettingsToJObject());
            }
            catch
            {
                Console.Write("Faild to Save");
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
                InitGlobal();
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
            if (option == ID.ST_isDark) SettingsData.isDark = Convert.ToBoolean(value);
            else if (option == ID.ST_ThemeColor) SettingsData.ThemeColor = value;
            else if (option == ID.ST_Language) SettingsData.Laguage = value;
            else if (option == ID.ST_TESVPath) SettingsData.TESVPath = value;
            else if (option == ID.ST_StoragePath) SettingsData.StoragePath = value;
            else return false;

            return true;
        }

        /// <summary>
        /// 初始化全局设置
        /// </summary>
        private static bool InitGlobal()
        {
            try
            {
                SetGlobal(ID.ST_isDark, "false");
                SetGlobal(ID.ST_ThemeColor, "brown");
                SetGlobal(ID.ST_Language, ID.English);
                SetGlobal(ID.ST_StoragePath, Directory.GetCurrentDirectory());
                JsonHelper.JsonSave(Directory.GetCurrentDirectory() + "\\Setings.Json",TransGlobalSettingsToJObject());
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static JObject TransGlobalSettingsToJObject()
        {
            JObject jObject = new JObject
            {
                { ID.ST_isDark, SettingsData.isDark },
                { ID.ST_ThemeColor,SettingsData.ThemeColor},
                { ID.ST_Language,SettingsData.Laguage },
                { ID.ST_TESVPath,SettingsData.TESVPath },
                { ID.ST_StoragePath,SettingsData.StoragePath }
            };
            return jObject;
        }
    }
}
