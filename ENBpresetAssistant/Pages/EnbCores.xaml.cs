using ENBpresetAssistant.Data;
using ENBpresetAssistant.Tools;
using MaterialDesignThemes.Wpf;
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

namespace ENBpresetAssistant.Pages
{
    /// <summary>
    /// EnbCores.xaml 的交互逻辑
    /// </summary>
    public partial class EnbCores : UserControl
    {
        public EnbCores()
        {
            InitializeComponent();

            ShowCores();
        }

        #region Buttons Click

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string ZipFile = FileHelper.OpenFileDialog("Zip Files (*.zip)|*.zip");
            if (String.IsNullOrEmpty(ZipFile)) return;
            string ZipName = ZipFile.Substring(ZipFile.LastIndexOf("\\") + 1, ZipFile.LastIndexOf(".") - (ZipFile.LastIndexOf("\\") + 1));

            await FileHelper.TempUnzip(ZipFile);

            GlobalVariables_Core.Init_Variables();
            GlobalVariables_Core.ZipName = ZipName;


        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var thisButton = sender as Button;
                var thisCard = MainView.FindName(thisButton.Tag.ToString()) as Card;
                var Cores = CoreHelper.GetCoresFromJson();

                var thisCore = Cores.FirstOrDefault(p => p.CoreVersion == thisCard.Tag.ToString());

                FileHelper.RM_Folder(SettingsData.StoragePath + ID.Dir_Core + "\\" + thisCore.CoreVersion);

                Cores.Remove(thisCore);
                CoreHelper.SavePrests(Cores);

                RemoveFromView(thisButton.Tag.ToString());

                SB_Message("Success_CoreRemoved");
            }
            catch(Exception exp)
            {
                Console.WriteLine(exp);
                SB_Message("Error_FailedToDelete");
                return;
            }
        }

        #endregion

        private bool ShowCores()
        {
            var Cores = CoreHelper.GetCoresFromJson();
            if (Cores == null)
            {
                return false;
            }
            if (Cores.Count == 0)
            {
                return false;
            }

            var AllCards = CreateCards(Cores);

            if(AllCards==null)
            {
                SB_Message("Error_FailedToGetCores");
                CoreHelper.SavePrests(null);
                return false;
            }

            foreach (var thisCard in AllCards)
            {
                AddToView(thisCard, thisCard.Name);
            }


            return true;
        }

        #region Create Card

        private List<Card> CreateCards(List<CoreData> coreDatas)
        {
            try
            {
                List<Card> cards = new List<Card>();
                foreach (var item in coreDatas)
                {
                    cards.Add(CreateCard(item));
                }
                return cards;
            }
           catch(Exception exp)
            {
                Console.WriteLine(exp);
                return null;
            }
        }

        private Card CreateCard(CoreData coreData)
        {
            string UUID = Guid.NewGuid().ToString("N");
            while (!Char.IsLetter(UUID[0]))
            {
                UUID = UUID.Substring(1);
            }

            TextBlock CoreVersion = new TextBlock() { Text = coreData.CoreVersion, Style = (Style)this.FindResource("MaterialDesignTitleTextBlock") };
            TextBlock InstallTime = new TextBlock() { Text = coreData.InstallTime.ToString(), Style = (Style)this.FindResource("MaterialDesignBody1TextBlock") };
            TreeView FileTree = new TreeView() { Margin = new Thickness(10) };
            FileTree.Items.Clear();
            FileTree.Items.Add(TreeHelper.GetTreeViewItem(SettingsData.StoragePath + ID.Dir_Core + "\\" + coreData.CoreVersion, coreData.CoreVersion));
            Button DeleteBtn=new Button() { Style = (Style)this.FindResource("MaterialDesignFlatButton"), Content = LocalizedHelper.GetLocalizedString("Btn_Delete", ID.StrRes_Core), Margin = new Thickness(10),  Tag = UUID };
            DeleteBtn.Click += new RoutedEventHandler(DeleteBtn_Click);

            var Container = new DockPanel();
            Container.Children.Add(CoreVersion);
            Container.Children.Add(InstallTime);
            Container.Children.Add(FileTree);
            Container.Children.Add(DeleteBtn);

            Card card = new Card()
            {
                Background = (Brush)FindResource("PrimaryHueDarkBrush"),
                Foreground = (Brush)FindResource("PrimaryHueDarkForegroundBrush"),
                Padding = new Thickness(10),
                Content = Container,
                Tag=coreData.CoreVersion,
                Name=UUID
            };

            return card;
        }

        #endregion

        /// <summary>
        /// SnakeBar 消息
        /// </summary>
        /// <param name="MSG"></param>
        private void SB_Message(string MSG)
        {
            MainWindow.Snackbar.MessageQueue.Enqueue(LocalizedHelper.GetLocalizedString(MSG, ID.StrRes_Core));
        }

        #region Add And Remove Elements

        /// <summary>
        /// 向UI添加并注册组件
        /// </summary>
        /// <param name="uIElement"></param>
        /// <param name="elementID"></param>
        private void AddToView(UIElement uIElement, string elementID)
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
            if (uIElement != null)
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
