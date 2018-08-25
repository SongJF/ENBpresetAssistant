using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ENBpresetAssistant.Tools
{
    class TreeHelper
    {
        public static TreeViewItem GetTreeViewItem(string Path,string rootName)
        {
            TreeViewItem Nodes = new TreeViewItem() { Header = CreateTreeViewFolder(rootName) ,IsExpanded=true};
            getDirectories(Path, Nodes);
            getFiles(Path, Nodes);
            return Nodes;
        }

        /// <summary>
        /// 找到目标组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DependencyObject VisualUpwardSeach<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
            {
                source = VisualTreeHelper.GetParent(source);
            }
            return source;
        }

        /// <summary>
        /// 生成TreeView文件路径
        /// </summary>
        /// <param name="treeViewItem"></param>
        /// <returns></returns>
        public static string GetTreeRouter(TreeViewItem treeViewItem)
        {
            string FullRouter = "";
            while (treeViewItem.Parent.GetType().Name == "TreeViewItem")
            {
                FullRouter = "\\" + treeViewItem.Header.ToString() + FullRouter;
                treeViewItem = (TreeViewItem)treeViewItem.Parent;
            }
            return FullRouter;
        }

        private static object CreateTreeViewFolder(string AValue)
        {
            return AValue;
        }

        private static void getDirectories(string Path,TreeViewItem treeViewItemNode)
        {
            string[] directories = Directory.GetDirectories(Path);

            foreach(var item in directories)
            {
                DirectoryInfo info = new DirectoryInfo(item);
                TreeViewItem Node = new TreeViewItem()
                {
                    Header = CreateTreeViewFolder(info.Name),
                };

                treeViewItemNode.Items.Add(Node);

                if (Directory.GetDirectories(item) != null) getDirectories(item, Node);    //遍历以搜索整个文件夹

                getFiles(item, Node);
            }
        }

        private static void getFiles(string Path, TreeViewItem treeViewItemNode)
        {
            string[] Files = Directory.GetFiles(Path);

            foreach (var item in Files)
            {
                TreeViewItem Node = new TreeViewItem() { Header = CreateTreeViewFolder(System.IO.Path.GetFileName(item)) };

                treeViewItemNode.Items.Add(Node);
            }
        }
    }
}
