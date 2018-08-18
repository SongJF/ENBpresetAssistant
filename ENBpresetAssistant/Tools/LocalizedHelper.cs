using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using WPFLocalizeExtension.Extensions;

namespace ENBpresetAssistant.Tools
{
    class LocalizedHelper
    {
        /// <summary>
        /// 获取本地化的字符串
        /// </summary>
        /// <param name="key">欲获取资源的Key</param>
        /// <param name="resourceFileName">资源文件名称</param>
        /// <param name="addSpaceAfter"></param>
        /// <returns></returns>
        public static string GetLocalizedString(string key, string resourceFileName)
        {
            var localizedString = String.Empty;

            // Build up the fully-qualified name of the key
            var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var fullKey = assemblyName + ":" + resourceFileName + ":" + key;
            var locExtension = new LocExtension(fullKey);
            locExtension.ResolveLocalizedValue(out localizedString);

            return localizedString;
        }

        public static bool ChangeLanguage(string lang)
        {
            try
            {
                WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = System.Globalization.CultureInfo.GetCultureInfo(lang);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
