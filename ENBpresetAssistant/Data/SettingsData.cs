namespace ENBpresetAssistant.Data
{
    public class SettingsData
    {
        private static bool _isDark;
        public static bool isDark
        {
            get { return _isDark; }
            set { _isDark = value; }
        }

        private static string _ThemeColor;
        public static string ThemeColor
        {
            get { return _ThemeColor; }
            set { _ThemeColor = value; }
        }

        private static string _Language;
        public static string Laguage
        {
            get { return _Language; }
            set { _Language = value; }
        }


        private static string _TESVPath;
        public static string TESVPath
        {
            get { return _TESVPath; }
            set { _TESVPath = value; }
        }

        private static string _StoragePath;
        public static string StoragePath
        {
            get { return _StoragePath; }
            set { _StoragePath = value; }
        }
    }
}
