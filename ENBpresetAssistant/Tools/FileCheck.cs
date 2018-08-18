using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBpresetAssistant.Tools
{
    class FileCheck
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

        public static bool PathAvailableOrNot(string Path)
        {
            if (Directory.Exists(Path)) return true;

            return false;
        }

        public static bool CreateFolder(string Path)
        {
            try
            {
                Directory.CreateDirectory(Path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
