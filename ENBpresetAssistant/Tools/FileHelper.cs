﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBpresetAssistant.Tools
{
    class FileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Files">欲查询的所有文件的文件名</param>
        /// <param name="Path">查询的路径</param>
        /// <param name="mode">匹配一个或全匹配</param>
        /// <returns></returns>
        public static bool FileExistOrNot(List<string> Files,string Path,int mode)
        {
            if (!Directory.Exists(Path)) return false;
            
            if(mode==0)
            {
                foreach (var FileName in Files)
                {
                    string FullName = Path + "\\" + FileName;
                    if (File.Exists(FullName)) return true;
                }
                return false;
            }

            foreach (var FileName in Files)
            {
                string FullName = Path + "\\" + FileName;
                if (!File.Exists(FileName)) return false;
            }
            return true;
        }

        public static bool CreateEmptyFolder(string Path)
        {
            try
            {
                if (PathAvailableOrNot(Path)) Directory.Delete(Path,true);

                CreateFolder(Path);
            }
            catch(Exception e)
            {
                Console.Write(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断路径存在与否
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static bool PathAvailableOrNot(string Path)
        {
            if (Directory.Exists(Path)) return true;

            return false;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static bool CreateFolder(string Path)
        {
            try
            {
                Directory.CreateDirectory(Path);
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <returns></returns>
        public static string OpenFolderDialog()
        {
            System.Windows.Forms.FolderBrowserDialog m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = m_Dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return null;
            }
            return m_Dialog.SelectedPath.Trim();
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <returns></returns>
        public static string OpenFileDialog(string FliterStr)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = FliterStr
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }
    }
}