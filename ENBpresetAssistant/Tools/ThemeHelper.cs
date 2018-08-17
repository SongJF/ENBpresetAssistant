using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using ENBpresetAssistant.Data;

namespace ENBpresetAssistant.Tools
{
    public class ThemeHelper
    {
        /// <summary>
        /// 设置整体主题
        /// </summary>
        public static bool ApplyTheme()
        {
            try
            {
                new PaletteHelper().SetLightDark(SettingsData.isDark);
                new PaletteHelper().ReplacePrimaryColor(SettingsData.ThemeColor);
                return true;
            }
            catch(Exception e)
            {
                Console.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 设置主题明暗
        /// </summary>
        /// <param name="isDark"></param>
        public static bool ApplyBase(bool isDark)
        {
            try
            {
                new PaletteHelper().SetLightDark(isDark);
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 设置主色
        /// </summary>
        /// <param name="swatch"></param>
        public static bool ApplyPrimary(Swatch swatch)
        {
            try
            {
                new PaletteHelper().ReplacePrimaryColor(swatch);
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }
        }
    }
}
