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
using System.Windows.Controls.Primitives;

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

            if(!SettingsHelper.ModifySettings("ThemeColor", SwatchColor.Name))
            {
                Failed();
                return;
            }

            Succeed();
        }

        private void BackGround_Click(object sender, RoutedEventArgs e)
        {
            bool isDark;
            var thisToggle = sender as ToggleButton;
            switch (thisToggle.IsChecked)
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

            if (!SettingsHelper.ModifySettings("isDark",isDark.ToString()))
            {
                Failed();
                return;
            }

            Succeed();
        }


        private void VerificationMode_Click(object sender, RoutedEventArgs e)
        {
            bool VerificationMode;
            var thisToggle = sender as ToggleButton;
            switch (thisToggle.IsChecked)
            {
                case true:
                    VerificationMode = true;
                    break;
                case false:
                    VerificationMode = false;
                    break;
                default:
                    VerificationMode = true;
                    break;
            }

            if (!SettingsHelper.ModifySettings(Data.ID.ST_VerificationMode, VerificationMode.ToString()))
            {
                Failed();
                return;
            }

            Succeed();
        }

        private void TESVPath_Click(object sender, RoutedEventArgs e)
        {
            string Path = FileHelper.OpenFolderDialog();
            if (Path == null) return;

            if (!PathCheck(Path)) return;
            if (Data.SettingsData.VerificationMode&&(!TESVCheck(Path))) return;
            if (!SettingsHelper.ModifySettings("TESVPath", Path)) return;

            TESVPath.Text = Path;

            Succeed();
        }

        private void StoragePath_Click(object sender, RoutedEventArgs e)
        {
            string Path = FileHelper.OpenFolderDialog();
            if (Path == null) return;

            if (!PathCheck(Path))
            {
                if (!(FileHelper.CreateFolder(Path)))
                {
                    MainWindow.Snackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("Failed_To_Create_New_Folder", "SettingsStr"));
                    return;
                }
                MainWindow.Snackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("Folder_Created", "SettingsStr"));
            }
            try
            {
                FileHelper.MV_Folder(Data.SettingsData.StoragePath + Data.ID.Dir_Storage, Path + Data.ID.Dir_Storage);
            }
            catch
            {
                StoragePath.Text = Data.SettingsData.StoragePath;
                Failed();
                return;
            }
            if (!SettingsHelper.ModifySettings("StoragePath", Path)) return;

            StoragePath.Text = Path;

            Succeed();
        }

        private void TESVTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var thisTexBox = sender as TextBox;
            string Path = thisTexBox.Text;
            if (Path == null) 
            {
                thisTexBox.Text = Data.SettingsData.TESVPath;
                return;
            }

            if (!PathCheck(Path))
            {
                thisTexBox.Text = Data.SettingsData.TESVPath;
                return;
            }
            if (Data.SettingsData.VerificationMode && (!TESVCheck(Path)))
            {
                thisTexBox.Text = Data.SettingsData.TESVPath;
                return;
            }

            if (! SettingsHelper.ModifySettings(thisTexBox.Name,thisTexBox.Text))
            {
                thisTexBox.Text = Data.SettingsData.TESVPath;
                Failed();
                return;
            }

            Succeed();
        }

        private void StorageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var thisTexBox = sender as TextBox;
            string Path = thisTexBox.Text;
            if (Path == null)
            {
                thisTexBox.Text = Data.SettingsData.StoragePath;
                return;
            }

            if (!PathCheck(Path))
            {
                if (!(FileHelper.CreateFolder(Path)))
                {
                    thisTexBox.Text = Data.SettingsData.StoragePath;
                    MainWindow.Snackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("Failed_To_Create_New_Folder", "SettingsStr"));
                    return;
                }
                MainWindow.Snackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("Folder_Created", "SettingsStr"));
            }

            try
            {
                FileHelper.MV_Folder(Data.SettingsData.StoragePath + Data.ID.Dir_Storage, Path + Data.ID.Dir_Storage);
            }
            catch
            {
                StoragePath.Text = Data.SettingsData.StoragePath;
                Failed();
                return;
            }

            if (!SettingsHelper.ModifySettings(thisTexBox.Name, thisTexBox.Text))
            {
                thisTexBox.Text = Data.SettingsData.StoragePath;
                Failed();
                return;
            }

            Succeed();
        }

        private void Succeed()
        {
            MainWindow.Snackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("Settins_Saved", Data.ID.StrRes_Settings));
        }

        private void Failed()
        {
            MainWindow.Snackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("Failed_To_Save", Data.ID.StrRes_Settings));
        }

        private bool PathCheck(string Path)
        {
            if (!FileHelper.PathAvailableOrNot(Path))
            {
                MainWindow.Snackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("Path_Does_Not_Exist", Data.ID.StrRes_Settings));
                return false;
            }

            return true;
        }

        private bool TESVCheck(string Path)
        {
            List<string> TESV = new List<string>
                {
                    Data.ID.Exe_Skyrim,
                    Data.ID.Exe_SkyrimSE,
                    "SkyrimSELauncher.exe",
                    "SkyrimLauncher.exe",
                    "TSEV.exe"
                };
            if (!FileHelper.FileExistOrNot(TESV, Path, 0))
            {
                MainWindow.Snackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("Not_TESV_Folder", Data.ID.StrRes_Settings));
                return false;
            }

            return true;
        }


        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            var thisCombo = sender as ComboBox;
            try
            {
                switch (thisCombo.SelectionBoxItem.ToString())
                {
                    case "中文":
                        LocalizedHelper.ChangeLanguage(Data.ID.Chinense);
                        SettingsHelper.ModifySettings(Data.ID.ST_Language, Data.ID.Chinense);
                        break;
                    case "English":
                        LocalizedHelper.ChangeLanguage(Data.ID.English);
                        SettingsHelper.ModifySettings(Data.ID.ST_Language, Data.ID.English);
                        break;
                    default:
                        return;
                }
            }
            catch
            {
                Failed();
                return;
            }

            Succeed();
        }
    }
}
