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
        public static void ApplyTheme()
        {
            new PaletteHelper().SetLightDark(SettingsData.isDark);
            new PaletteHelper().ReplacePrimaryColor(SettingsData.ThemeColor);
        }

        /// <summary>
        /// 设置主题明暗
        /// </summary>
        /// <param name="isDark"></param>
        public static void ApplyBase(bool isDark)
        {
            new PaletteHelper().SetLightDark(isDark);
        }

        /// <summary>
        /// 设置主色
        /// </summary>
        /// <param name="swatch"></param>
        public static void ApplyPrimary(Swatch swatch)
        {
            new PaletteHelper().ReplacePrimaryColor(swatch);
        }
    }
}
