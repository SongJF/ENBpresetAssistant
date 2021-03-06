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

using MaterialDesignThemes.Wpf;
using System.Threading;
using System.Windows.Controls.Primitives;
using ENBpresetAssistant.Domain;
using ENBpresetAssistant.Tools;

namespace ENBpresetAssistant
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow :Window
    {
        public static Snackbar Snackbar;
        public static Window GlobalMainWindow;
        public MainWindow()
        {
            InitializeComponent();

            Snackbar = this.MainSnackbar;
            GlobalMainWindow = this;

            InitSettings();

            DataContext = new MainWindowViewModel(Welcome());  //数据绑定

        }

        /// <summary>
        /// 开始欢迎
        /// </summary>
        private SnackbarMessageQueue Welcome()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
            }).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("WelcomMSG", Data.ID.StrRes_Main));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            return MainSnackbar.MessageQueue;
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void DialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;

            Application.Current.Shutdown();
        }

        /// <summary>
        /// 窗口与拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MenuListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }

        /// <summary>
        /// 初始化设置
        /// </summary>
        private void InitSettings()
        {
            if (!SettingsHelper.ReadSettings())
            {
                MainSnackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("Failed_To_Get_Settings", Data.ID.StrRes_Main));
            }
            if (!ThemeHelper.ApplyTheme()) MainSnackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString("Failed_To_Apply_Theme", Data.ID.StrRes_Main));

            LocalizedHelper.ChangeLanguage(Data.SettingsData.Laguage);
        }
    }
}
