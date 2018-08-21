using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using Newtonsoft.Json;
using ENBpresetAssistant.Data;
using Newtonsoft.Json.Linq;

namespace ENBpresetAssistant.Tools
{
    class JsonHelper
    {
        /// <summary>
        /// 从本地读取Json文件
        /// </summary>
        /// <param name="SettingsPath">Json文件路径</param>
        /// <returns></returns>
        public static string GetJsonFromFile(string SettingsPath)
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
        /// 传入JObject保存为Json文件
        /// </summary>
        /// <param name="SettingsPath">保存路径</param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public static bool JsonSave(string SettingsPath,JObject jObject)
        {
            try
            {
                File.WriteAllText(SettingsPath, GetJsonInString(jObject));
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 传入String保存为Json文件
        /// </summary>
        /// <param name="SettingsPath">保存路径</param>
        /// <param name="JsonStr"></param>
        /// <returns></returns>
        public static bool JsonSave(string SettingsPath, string JsonStr)
        {
            try
            {
                File.WriteAllText(SettingsPath, JsonStr);
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
        public static string GetJsonInString(JObject jObject)
        {
            StringWriter JsonString = new StringWriter();
            JsonWriter jw = new JsonTextWriter(JsonString);


            jw.WriteStartObject();
            if(jObject!=null)
            {
                foreach (var Object in jObject)
                {
                    jw.WritePropertyName(Object.Key);
                    jw.WriteValue(Object.Value);
                }
            }
            jw.WriteEndObject();
            jw.Flush();

            return JsonString.GetStringBuilder().ToString();
        }

        /// <summary>
        /// 把string类型的Json转化为JObject
        /// </summary>
        /// <param name="JsonString"></param>
        /// <returns></returns>
        public static JObject TransStrToJObject(string JsonString)
        {
            return (JObject)JsonConvert.DeserializeObject(JsonString);
        }
    }
}
