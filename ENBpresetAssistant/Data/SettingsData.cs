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


        private static string _TESVPath;
        public static string TESVPath
        {
            get { return _TESVPath; }
            set { _TESVPath = value; }
        }

        private static string _ENBPresetPath;
        public static string ENBPresetPath
        {
            get { return _ENBPresetPath; }
            set { _ENBPresetPath = value; }
        }

        private static string _ENBCoresPath;
        public static string ENBCoresPath
        {
            get { return _ENBCoresPath; }
            set { _ENBCoresPath = value; }
        }
    }
}
