﻿using System;
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

            if (!SettingsHelper.ModifySettings("ThemeColor",isDark.ToString()))
            {
                Failed();
                return;
            }

            Succeed();
        }

        private void TESVPath_Click(object sender, RoutedEventArgs e)
        {
            string Path = OpenFolderDialog();
            if (Path == null) return;

            if (!PathCheck(Path)) return;
            if (!TESVCheck(Path)) return;
            if (!SettingsHelper.ModifySettings("TESVPath", Path)) return;

            TESVPath.Text = Path;

            Succeed();
        }

        private void StoragePath_Click(object sender, RoutedEventArgs e)
        {
            string Path = OpenFolderDialog();
            if (Path == null) return;

            if (!PathCheck(Path)) return;
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
            if (!TESVCheck(Path))
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
                thisTexBox.Text = Data.SettingsData.StoragePath;
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

        private void Succeed()
        {
            MainWindow.Snackbar.MessageQueue.Enqueue("Settins Saved");
        }

        private void Failed()
        {
            MainWindow.Snackbar.MessageQueue.Enqueue("Failed To Save");
        }

        private bool PathCheck(string Path)
        {
            if (!FileCheck.PathAvailableOrNot(Path))
            {
                MainWindow.Snackbar.MessageQueue.Enqueue("Unvalid Path");
                return false;
            }

            return true;
        }

        private bool TESVCheck(string Path)
        {
            List<string> TESV = new List<string>
                {
                    "Skyrim.exe",
                    "SkyrimSE.exe"
                };
            if (!FileCheck.FileExistOrNot(TESV, Path, 0))
            {
                MainWindow.Snackbar.MessageQueue.Enqueue("Not TESV Folder");
                return false;
            }

            return true;
        }
    }
}
