using System;
using System.Collections.Generic;
using System.IO;
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
using ENBpresetAssistant.Components;
using ENBpresetAssistant.Data;
using ENBpresetAssistant.Tools;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json.Linq;

namespace ENBpresetAssistant.Pages
{
    /// <summary>
    /// EnbPresets.xaml 的交互逻辑
    /// </summary>
    public partial class EnbPresets : UserControl
    {
        private List<PresetData> Presets;

        public EnbPresets()
        {
            InitializeComponent();

            ShowPresets();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var thisButton = sender as Button;
            RemoveFromView(thisButton.Tag.ToString());
        }

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
                AddToView(newFlipper, newFlipper.Tag.ToString());

                SB_Message("Preset_Added");
            }
        }

        /// <summary>
        /// 展示ENB预设
        /// </summary>
        /// <returns></returns>
        private bool ShowPresets()
        {
            if (!CheckTESVFolerState())
            {
                ShowExpectionText("Unmanaged_ENB");
                return false;
            }

            Presets = PresetHelper.GetPresetFromJson();
            if(Presets==null)
            {
                SB_Message("No_Preset_Managed");
                PresetHelper.InitPresetJson();
                return false;
            } 
            if (Presets.Count == 0)
            {
                SB_Message("No_Preset_Managed");
                return false;
            }

            var Flippers = CreateFlippers(Presets);

            foreach(var Flipper in Flippers)
            {
                AddToView(Flipper, Flipper.Tag.ToString());
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
                Tag = UUID,
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

            Button DetailBtn = new Button() { Style = (Style)this.FindResource("MaterialDesignFlatButton"), Content = LocalizedHelper.GetLocalizedString("Btn_Detail", ID.StrRes_Preset), Margin = new Thickness(10), Command = Flipper.FlipCommand ,Tag=UUID};

            Button ChangeStateBtn = new Button() { Style = (Style)this.FindResource("MaterialDesignFloatingActionMiniAccentButton"), Content = new PackIcon() { Kind = PackIconKind.Upload }, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 16, -25) ,Tag=UUID};

            if (preset.isRunning)
            {
                TitlecolorZone.Mode = ColorZoneMode.PrimaryDark;
                StateText.Foreground = (Brush)this.FindResource("AccentColorBrush");
                StateText.Text = LocalizedHelper.GetLocalizedString("State_Running", ID.StrRes_Preset);
                ChangeStateBtn.Content = new PackIcon() { Kind = PackIconKind.Download };
            }

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
