using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


using ENBpresetAssistant.Tools;
using MaterialDesignThemes.Wpf.Transitions;

namespace ENBpresetAssistant.Pages.Transitions.PresetAdd
{
    /// <summary>
    /// Intro1.xaml 的交互逻辑
    /// </summary>
    public partial class Slider1_Intro : UserControl
    {
        public Slider1_Intro()
        {
            InitializeComponent();

            DataContext = new Data.GlobalVariables_Preset();

            DirectotyTree_Load(Directory.GetCurrentDirectory() + Data.ID.Dir_Temp , Data.GlobalVariables_Preset.ZipName);
            Init_ComboBox();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {

            Transitioner.MoveNextCommand.Execute(null, null);
        }

        private void Init_ComboBox()
        {
            var Cores = CoreHelper.GetCoresFromJson();
            if(Cores!=null)
            {
                foreach (var item in Cores)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem()
                    {
                        Content = item.CoreVersion
                    };
                    CoreSelection.Items.Add(comboBoxItem);
                }
            }
        }

        private void ShowPresetAvailability(string Path)
        {
            List<string> ENBFiles = new List<string>()
            {
                "enblens.fx",
                "enbeffect.fx",
                "enbbloom.fx",
                "enblocal.ini",
                "enbseries.ini"
            };
            if(FileHelper.FileExistOrNot(ENBFiles, Path))
            {
                PresetAvailability.Text = LocalizedHelper.GetLocalizedString("Intro_Preset_Available", Data.ID.StrRes_Preset);
                PresetAvailability.Foreground = new SolidColorBrush(Colors.Green);
                return;
            }

            PresetAvailability.Text = LocalizedHelper.GetLocalizedString("Intro_Preset_Unavailable", Data.ID.StrRes_Preset);
            PresetAvailability.Foreground = new SolidColorBrush(Colors.Red);
        }

        /// <summary>
        /// 加载文件树
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="rootName"></param>
        private void DirectotyTree_Load(string Path,string rootName)
        {
            DirectotyTree.Items.Clear();
            
            DirectotyTree.Items.Add(TreeHelper.GetTreeViewItem(Path,rootName));

            ShowPresetAvailability(Path);
        }


        /// <summary>
        /// 文件树右键操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirectotyTree_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = VisualUpwardSeach < TreeViewItem >((DependencyObject)e.OriginalSource) as TreeViewItem;
            if (treeViewItem != null)
            {
                string FullTreeRouter = GetTreeRouter(treeViewItem);

                if (String.IsNullOrEmpty(FullTreeRouter)) return;

                treeViewItem.ContextMenu = CreateMenu(FullTreeRouter);

                treeViewItem.Focus();

                e.Handled = true;

            }
        }

        /// <summary>
        /// 找到右键的目标组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private DependencyObject VisualUpwardSeach<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
            {
                source = VisualTreeHelper.GetParent(source);
            }
            return source;
        }

        /// <summary>
        /// 创建右键菜单
        /// </summary>
        /// <param name="Router"></param>
        /// <returns></returns>
        private ContextMenu CreateMenu(string Router)
        {
            ContextMenu menu = new ContextMenu();

            MenuItem menuItem = new MenuItem() { Header = LocalizedHelper.GetLocalizedString("Intro_SetRoot", Data.ID.StrRes_Preset),Tag= Router};
            menuItem.Click += new RoutedEventHandler(SetRootMenu_Click);
            menu.Items.Add(menuItem);

            return menu;
        }

        /// <summary>
        /// 变换根节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetRootMenu_Click(object sender, RoutedEventArgs e)
        {
            var thisMenuItem = sender as MenuItem;
            if (thisMenuItem == null) return;
            var TreeItems = DirectotyTree.Items;
            try
            {
                DirectotyTree_Load(Directory.GetCurrentDirectory() + Data.ID.Dir_Temp + "\\" + thisMenuItem.Tag,thisMenuItem.Tag.ToString());
            }
            catch(Exception expecion)
            {
                DirectotyTree_Load(Directory.GetCurrentDirectory() + Data.ID.Dir_Temp, Data.GlobalVariables_Preset.ZipName);
                Data.GlobalVariables_Preset.RouterInTemp = "";
                Console.Write(expecion);
                return;
            }
            Data.GlobalVariables_Preset.RouterInTemp += "\\" + thisMenuItem.Tag;
        }

        /// <summary>
        /// 生成TreeView文件路径
        /// </summary>
        /// <param name="treeViewItem"></param>
        /// <returns></returns>
        private string GetTreeRouter(TreeViewItem treeViewItem)
        {
            string FullRouter = "";
            while(treeViewItem.Parent.GetType().Name == "TreeViewItem")
            {
                FullRouter = "\\" + treeViewItem.Header.ToString() + FullRouter;
                treeViewItem = (TreeViewItem)treeViewItem.Parent;
            }
            return FullRouter;
        }
    }
}
