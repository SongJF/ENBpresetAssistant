using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ENBpresetAssistant.Data;
using ENBpresetAssistant.Tools;
using MaterialDesignThemes.Wpf;

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

            ShowPresets();
        }

        #region Buttons Click

        /// <summary>
        /// 删除该组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var thisButton = sender as Button;
            RemoveFromView(thisButton.Tag.ToString());
        }

        /// <summary>
        /// 应用该预设
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyPresetBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(SettingsData.TESVPath))
                {
                    SB_Message("Error_TESVPathUnset");
                    return;
                }

                var CurrentPresets = PresetHelper.GetPresetFromJson();
                if (CurrentPresets == null) return;
                if (CurrentPresets.FirstOrDefault(p => p.isRunning == true) != null)
                {
                    SB_Message("Error_ENBRunning");
                    return;
                }

                var thisButton = sender as Button;
                Flipper thisFlipper = MainView.FindName(thisButton.Tag.ToString()) as Flipper;
                PresetData preset = CurrentPresets.FirstOrDefault(p => p.PresetName == thisFlipper.Tag.ToString());
                if (preset == null)
                {
                    SB_Message("Error_PresetNotFound");
                    return;
                }
                CurrentPresets.Remove(preset);
                preset.isRunning = true;
                CurrentPresets.Add(preset);


                FileHelper.CP_Folder(SettingsData.StoragePath + ID.Dir_Preset+ "\\" + preset.PresetName, SettingsData.TESVPath);

                PresetHelper.SavePrests(CurrentPresets);

                var newFlipper = CreateFlipper(preset);
                MainView.Children.Insert(0, newFlipper);
                MainView.RegisterName(newFlipper.Name, newFlipper);
                RemoveFromView(thisButton.Tag.ToString());

                SB_Message("Success_PresetApplyed");
            }
            catch(Exception exp)
            {
                Console.Write(exp);
                return;
            }
        }

        /// <summary>
        /// 解除该预设的应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnApplyPresetBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SettingsData.TESVPath == null)
                {
                    SB_Message("Error_TESVPathUnset");
                    return;
                }

                var CurrentPresets = PresetHelper.GetPresetFromJson();
                if (CurrentPresets == null) return;

                var thisButton = sender as Button;
                Flipper thisFlipper = MainView.FindName(thisButton.Tag.ToString()) as Flipper;
                PresetData preset = CurrentPresets.FirstOrDefault(p => p.PresetName == thisFlipper.Tag.ToString());
                if (preset == null)
                {
                    SB_Message("Error_PresetNotFound");
                    return;
                }
                CurrentPresets.Remove(preset);


                preset.isRunning = false;
                CurrentPresets.Add(preset);
                var newFlipper = CreateFlipper(preset);
                AddToView(newFlipper, newFlipper.Name);
                RemoveFromView(thisButton.Tag.ToString());

                FileHelper.RM_Folder(SettingsData.StoragePath + ID.Dir_Preset+ "\\" + preset.PresetName, SettingsData.TESVPath);

                PresetHelper.SavePrests(CurrentPresets);

                SB_Message("Success_PresetUnApplyed");
            }
            catch (Exception exp)
            {
                Console.Write(exp);
                return;
            }
        }

        private void DetailBtn_Click(object sender, RoutedEventArgs e)
        {
            var thisButton = sender as Button;
            Flipper thisFlipper = MainView.FindName(thisButton.Tag.ToString()) as Flipper;

            var CurrentPresets = PresetHelper.GetPresetFromJson();
            PresetData preset = CurrentPresets.FirstOrDefault(p => p.PresetName == thisFlipper.Tag.ToString());

            thisFlipper.BackContent = (CreateBackContent(preset,thisFlipper.Name));

            Flipper.FlipCommand.Execute(null, null);
        }

        /// <summary>
        /// 添加Preset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string ZipFile = FileHelper.OpenFileDialog("Zip Files (*.zip)|*.zip");
            if (String.IsNullOrEmpty(ZipFile)) return;
            string ZipName = ZipFile.Substring(ZipFile.LastIndexOf("\\") + 1, ZipFile.LastIndexOf(".") - (ZipFile.LastIndexOf("\\") + 1));

            await PresetHelper.TempUnzip(ZipFile);

            GlobalVariables_Preset.Init_Variables();
            GlobalVariables_Preset.ZipName = ZipName;

            var addPresetIntro = new InstallWin.PresetAdd.PresetInstall();
            addPresetIntro.Owner = MainWindow.GlobalMainWindow;
            addPresetIntro.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addPresetIntro.ShowDialog();

            if (GlobalVariables_Preset.AddComplete)
            {
                PresetData presetData = new PresetData()
                {
                    PresetName = GlobalVariables_Preset.PresetName,
                    Core = GlobalVariables_Preset.CoreVersion,
                    InstallTime = DateTime.UtcNow,
                    isRunning = false
                };
                PresetHelper.AddPresetJson(presetData);

                var newFlipper = CreateFlipper(presetData);
                RemoveFromView(ID.Preset_ExpText);
                AddToView(newFlipper, newFlipper.Name);

                SB_Message("Preset_Added");
            }
        }

        #endregion

        /// <summary>
        /// 展示ENB预设
        /// </summary>
        /// <returns></returns>
        private bool ShowPresets()
        {
            var Presets = PresetHelper.GetPresetFromJson();
            if(Presets==null)
            {
                if (!CheckTESVFolerState())
                {
                    ShowExpectionText("Unmanaged_ENB");
                    return false;
                }
                SB_Message("No_Preset_Managed");
                return false;
            } 
            if (Presets.Count == 0)
            {
                SB_Message("No_Preset_Managed");
                return false;
            }

            var AllFlippers = CreateFlippers(Presets);
            var ApplyedPreset = Presets.FirstOrDefault(p => p.isRunning == true);

            if(ApplyedPreset!=null)
            {
                var ApplyedFlipper = AllFlippers.FirstOrDefault(p => p.Tag.ToString() == ApplyedPreset.PresetName);
                AddToView(ApplyedFlipper, ApplyedFlipper.Name);
                AllFlippers.Remove(ApplyedFlipper);
            }

            foreach (var thisFlipper in AllFlippers)
            {
                AddToView(thisFlipper, thisFlipper.Name);
            }


            return true;
        }

        #region 提取方法

        /// <summary>
        /// 预设不可用警示文本
        /// </summary>
        /// <param name="ErroMSG"></param>
        /// <returns></returns>
        private void ShowExpectionText(string ErroMSG,Style style= null)
        {
            if (style == null) style = (Style)FindResource("MaterialDesignDisplay3TextBlock");
            TextBlock textBlock = new TextBlock()
            {
                Text = LocalizedHelper.GetLocalizedString("No_Preset_Managed", ID.StrRes_Preset),
                Style = (Style)this.FindResource("MaterialDesignDisplay3TextBlock"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = (Brush)this.FindResource("AccentColorBrush2"),
                TextWrapping=TextWrapping.Wrap
            };

            AddToView(textBlock, ID.Preset_ExpText);
        }

        /// <summary>
        /// 检测是否有未经管理的ENB正在被使用
        /// </summary>
        /// <returns></returns>
        private bool CheckTESVFolerState()
        {
            List<string> Enbitems = new List<string>()
            {
                "d3d9.dll",
                "d3d11.dll",
                "enbbloom.fx",
                "enbeffect.fx",
                "enblens.fx",
                "enblocal.ini",
                "enbseries.ini"
            };

            if (FileHelper.FileExistOrNot(Enbitems, SettingsData.TESVPath, 0)) return false;

            return true;
        }

        /// <summary>
        /// SnakeBar 消息
        /// </summary>
        /// <param name="MSG"></param>
        private void SB_Message(string MSG)
        {
            MainWindow.Snackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString(MSG, ID.StrRes_Preset));
        }

        #endregion

        #region Create Flipper

        /// <summary>
        /// 创建Flipper
        /// </summary>
        /// <param name="presetDatas"></param>
        /// <returns></returns>
        private List<Flipper> CreateFlippers(List<PresetData> presetDatas)
        {
            List<Flipper> cards = new List<Flipper>();

            foreach(var preset in presetDatas)
            {
                cards.Add(CreateFlipper(preset));
            }

            return cards;
        }

        private Flipper CreateFlipper(PresetData preset)
        {
            string UUID = Guid.NewGuid().ToString("N");
            while(!Char.IsLetter(UUID[0]))
            {
                UUID = UUID.Substring(1);
            }
            Flipper flipper = new Flipper()
            {
                Style = (Style)this.FindResource("MaterialDesignCardFlipper"),
                Margin = new Thickness(30, 20, 30, 20),
                FrontContent = CreateFrontContent(preset,UUID),
                Name=UUID,
                Tag = preset.PresetName,
                Width=180,
                HorizontalContentAlignment=HorizontalAlignment.Center,
                VerticalContentAlignment=VerticalAlignment.Top
            };

            return flipper;
        }

        /// <summary>
        /// 创建Flipper的前展示区
        /// </summary>
        /// <param name="preset"></param>
        /// <returns></returns>
        private UIElement CreateFrontContent(PresetData preset,string UUID)
        {
            ColorZone TitlecolorZone = new ColorZone() { Mode = ColorZoneMode.PrimaryLight, Height = 30 };
            TextBlock StateText = new TextBlock() { Text = LocalizedHelper.GetLocalizedString("State_Available", ID.StrRes_Preset), Foreground = (Brush)this.FindResource("AccentColorBrush3"), Margin = new Thickness(10) };

            TextBlock NamText = new TextBlock() { Text = preset.PresetName, Margin = new Thickness(10), Style = (Style)this.FindResource("MaterialDesignTitleTextBlock"), HorizontalAlignment = HorizontalAlignment.Center };

            TextBlock CoreText = new TextBlock() { Text = preset.Core, Margin = new Thickness(10), Style = (Style)this.FindResource("MaterialDesignBody1TextBlock"), HorizontalAlignment = HorizontalAlignment.Center };

            TextBlock InstallTime = new TextBlock() { Text = preset.InstallTime.Date.ToString(), Margin = new Thickness(10, 10, 10, 10), Style = (Style)this.FindResource("MaterialDesignBody1TextBlock"), HorizontalAlignment = HorizontalAlignment.Center };

            Button DetailBtn = new Button() { Style = (Style)this.FindResource("MaterialDesignFlatButton"), Content = LocalizedHelper.GetLocalizedString("Btn_Detail", ID.StrRes_Preset), Margin = new Thickness(10) ,Tag=UUID};
            DetailBtn.Click += new RoutedEventHandler(DetailBtn_Click);

            Button ChangeStateBtn = new Button() { Style = (Style)this.FindResource("MaterialDesignFloatingActionMiniAccentButton"), Content = new PackIcon() { Kind = PackIconKind.Upload }, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 16, -25) ,Tag=UUID};

            if (preset.isRunning)
            {
                TitlecolorZone.Mode = ColorZoneMode.PrimaryDark;
                StateText.Foreground = (Brush)this.FindResource("AccentColorBrush");
                StateText.Text = LocalizedHelper.GetLocalizedString("State_Running", ID.StrRes_Preset);
                ChangeStateBtn.Content = new PackIcon() { Kind = PackIconKind.Download };
                ChangeStateBtn.Click += new RoutedEventHandler(UnApplyPresetBtn_Click);
            }
            else ChangeStateBtn.Click += new RoutedEventHandler(ApplyPresetBtn_Click);
            

            Grid TitleGrid = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition(),
                    new RowDefinition()
                }
            };
            TitleGrid.Children.Add(TitlecolorZone);
            TitleGrid.Children.Add(ChangeStateBtn);
            TitleGrid.Children.Add(NamText);
            Grid.SetRow(TitlecolorZone, 0);
            Grid.SetRow(ChangeStateBtn, 0);
            Grid.SetRow(NamText, 1);

            Grid InfoGrid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                },
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10)
            };
            InfoGrid.Children.Add(CoreText);
            InfoGrid.Children.Add(StateText);
            Grid.SetColumn(CoreText, 0);
            Grid.SetColumn(StateText, 1);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(TitleGrid);
            stackPanel.Children.Add(InfoGrid);
            stackPanel.Children.Add(InstallTime);
            stackPanel.Children.Add(DetailBtn);

            return stackPanel;
        }

        /// <summary>
        /// 创建Flipper的后展示区
        /// </summary>
        /// <param name="preset"></param>
        /// <returns></returns>
        private UIElement CreateBackContent(PresetData preset,string UUID)
        {
            ColorZone TitlecolorZone = new ColorZone() { Mode = ColorZoneMode.PrimaryLight, Height = 30 };

            TreeView FileTree = new TreeView() { Height=200 ,Margin=new Thickness(10)};
            FileTree.Items.Clear();
            FileTree.Items.Add(TreeHelper.GetTreeViewItem(SettingsData.StoragePath + ID.Dir_Preset + "\\" + preset.PresetName, preset.PresetName));

            Button DeleteBtn = new Button() { Style = (Style)this.FindResource("MaterialDesignFloatingActionMiniAccentButton"), Content = new PackIcon() { Kind = PackIconKind.Delete }, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 16, -25) ,Tag= UUID};
            DeleteBtn.Click += new RoutedEventHandler(DeleteBtn_Click);

            Button ReturnBtn = new Button() { Style = (Style)this.FindResource("MaterialDesignFlatButton"), Content = LocalizedHelper.GetLocalizedString("Btn_Return", ID.StrRes_Preset), Margin = new Thickness(10), Command = Flipper.FlipCommand ,Tag= UUID};

            if (preset.isRunning)
            {
                TitlecolorZone.Mode = ColorZoneMode.PrimaryDark;
            }

            Grid TitleGrid = new Grid();
            TitleGrid.Children.Add(TitlecolorZone);
            TitleGrid.Children.Add(DeleteBtn);

            StackPanel stackPanel = new StackPanel()
            {
                Children = { TitleGrid, FileTree, ReturnBtn }
            };

            return stackPanel;
        }

        #endregion

        #region Add And Remove Elements

        /// <summary>
        /// 向UI添加并注册组件
        /// </summary>
        /// <param name="uIElement"></param>
        /// <param name="elementID"></param>
        private void AddToView(UIElement uIElement,string elementID)
        {
            MainView.Children.Add(uIElement);
            MainView.RegisterName(elementID, uIElement);
        }

        /// <summary>
        /// 从UI中移除组件
        /// </summary>
        /// <param name="elementID"></param>
        /// <returns></returns>
        private bool RemoveFromView(string elementID)
        {
            UIElement uIElement = MainView.FindName(elementID) as UIElement;
            if(uIElement!=null)
            {
                MainView.Children.Remove(uIElement);
                MainView.UnregisterName(elementID);
                return true;
            }
            return false;
        }
        #endregion
    }
}
