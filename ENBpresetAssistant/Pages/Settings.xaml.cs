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


using MaterialDesignColors;
using ENBpresetAssistant.Tools;

namespace ENBpresetAssistant.Pages
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : UserControl
    {
        public IEnumerable<Swatch> Swatches { get; }
        public Settings()
        {
            InitializeComponent();

            Swatches = new SwatchesProvider().Swatches;

            DataContext = new Data.SettingsData();
        }

        private void Theme_Click(object sender, RoutedEventArgs e)
        {
            var thisButton = sender as Button;

            var SwatchColor = Swatches.FirstOrDefault(p => p.Name == thisButton.Name);

            if (SwatchColor == null) return;

            ThemeHelper.ApplyPrimary(SwatchColor);
        }

        private void BackGround_Click(object sender, RoutedEventArgs e)
        {
            bool isDark;
            switch(BG_Toggle.IsChecked)
            {
                case true:
                    isDark = true;
                    break;
                case false:
                    isDark = false;
                    break;
                default:
                    isDark = false;
                    break;
            }
            ThemeHelper.ApplyBase(isDark);
        }

        private void Path_Click(object sender, RoutedEventArgs e)
        {
            var thisButton = sender as Button;
            string Path = OpenFolderDialog();
            if (Path == null) return;

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private string OpenFolderDialog()
        {
            System.Windows.Forms.FolderBrowserDialog m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = m_Dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return null;
            }
            return m_Dialog.SelectedPath.Trim();
        }
    }
}
