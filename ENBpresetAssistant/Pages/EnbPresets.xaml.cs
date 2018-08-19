using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using ENBpresetAssistant.Data;
using ENBpresetAssistant.Tools;

namespace ENBpresetAssistant.Pages
{
    /// <summary>
    /// EnbPresets.xaml 的交互逻辑
    /// </summary>
    public partial class EnbPresets : UserControl
    {
        public EnbPresets()
        {
            InitializeComponent();

            ShowNoPresetsText();
        }

        private void GetAllPresets()
        {
            string PresetsConfigPath = SettingsData.StoragePath + "\\Presets.Json";

            string PresrtsJson = JsonHelper.GetJsonFromFile(PresetsConfigPath);

            
        }

        private void ShowNoPresetsText()
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = LocalizedHelper.GetLocalizedString("No_Preset_Managed", ID.StrRes_Preset),
                Style = (Style)this.FindResource("MaterialDesignDisplay3TextBlock"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = (Brush)this.FindResource("AccentColorBrush2")
            };
            MainGrid.Children.Add(textBlock);
        }
    }
}
