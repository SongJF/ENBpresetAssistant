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
using System.Windows.Shapes;


using ENBpresetAssistant.Data;
using ENBpresetAssistant.Tools;

namespace ENBpresetAssistant.Pages.InstallWin.CoreAdd
{
    /// <summary>
    /// CoreInstall.xaml 的交互逻辑
    /// </summary>
    public partial class CoreInstall : Window
    {
        public CoreInstall()
        {
            InitializeComponent();

            DataContext = new CoreInstallViewModel() { CoreVersion = GlobalVariables_Core.ZipName };

            DirectotyTree_Load(Directory.GetCurrentDirectory() + ID.Dir_Temp, GlobalVariables_Core.ZipName);
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

        /// <summary>
        /// 安装按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(CoreVersionText)) return;

            GlobalVariables_Core.CoreVersion = CoreVersionText.Text;

            FileHelper.MV_Folder(Directory.GetCurrentDirectory() + ID.Dir_Temp + GlobalVariables_Core.RouterInTemp, SettingsData.StoragePath + ID.Dir_Core + "\\" + GlobalVariables_Core.CoreVersion);
            GlobalVariables_Core.isCompelete = true;
            this.Close();
        }

        /// <summary>
        /// 判断目录是否可用
        /// </summary>
        /// <param name="Path"></param>
        private void ShowCoreAvailability(string Path)
        {
            List<string> ENBFiles = new List<string>()
            {
                "d3d9.dll",
                "d3d11.dll"
            };
            if (FileHelper.FileExistOrNot(ENBFiles, Path))
            {
                CoreAvailability.Text = LocalizedHelper.GetLocalizedString("Intro_CoreAvailable", ID.StrRes_Core);
                CoreAvailability.Foreground = new SolidColorBrush(Colors.Green);
                return;
            }

            CoreAvailability.Text = LocalizedHelper.GetLocalizedString("Intro_CoreUnavailable", ID.StrRes_Core);
            CoreAvailability.Foreground = new SolidColorBrush(Colors.Red);
        }

        #region FileTree Options

        /// <summary>
        /// 加载文件树
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="rootName"></param>
        private void DirectotyTree_Load(string Path, string rootName)
        {
            DirectotyTree.Items.Clear();

            DirectotyTree.Items.Add(TreeHelper.GetTreeViewItem(Path, rootName));

            ShowCoreAvailability(Path);
        }

        /// <summary>
        /// 文件树右键操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirectotyTree_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = TreeHelper.VisualUpwardSeach<TreeViewItem>((DependencyObject)e.OriginalSource) as TreeViewItem;
            if (treeViewItem != null)
            {
                string FullTreeRouter = TreeHelper.GetTreeRouter(treeViewItem);

                if (String.IsNullOrEmpty(FullTreeRouter)) return;

                treeViewItem.ContextMenu = CreateRootChangeMenu(FullTreeRouter);

                treeViewItem.Focus();

                e.Handled = true;

            }
        }

        /// <summary>
        /// 创建右键菜单
        /// </summary>
        /// <param name="Router"></param>
        /// <returns></returns>
        private ContextMenu CreateRootChangeMenu(string Router)
        {
            ContextMenu menu = new ContextMenu();

            MenuItem menuItem = new MenuItem() { Header = LocalizedHelper.GetLocalizedString("Intro_SetRoot", ID.StrRes_Core), Tag = Router };
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
                DirectotyTree_Load(Directory.GetCurrentDirectory() + ID.Dir_Temp + "\\" + thisMenuItem.Tag, thisMenuItem.Tag.ToString());
            }
            catch (Exception expecion)
            {
                DirectotyTree_Load(Directory.GetCurrentDirectory() + ID.Dir_Temp, GlobalVariables_Core.ZipName);
                GlobalVariables_Core.RouterInTemp = "";
                Console.Write(expecion);
                return;
            }
            GlobalVariables_Core.RouterInTemp += thisMenuItem.Tag;
        }
        #endregion

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
