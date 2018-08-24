using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ENBpresetAssistant.Tools
{
    class TreeHelper
    {
        public static TreeViewItem GetTreeViewItem(string Path,string rootName)
        {
            TreeViewItem Nodes = new TreeViewItem() { Header = CreateTreeViewFolder(rootName) };
            getDirectories(Path, Nodes);
            getFiles(Path, Nodes);
            return Nodes;
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
