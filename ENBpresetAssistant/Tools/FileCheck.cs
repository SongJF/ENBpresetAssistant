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
        public bool ExistOrNot(IEnumerable<string> Files,string Path,int mode)
        {
            if (!Directory.Exists(Path)) return false;
            
            if(mode==0)
            {
                foreach (var FileName in Files)
                {
                    string FullName = Path + "\\" + FileName;
                    if (File.Exists(FileName)) return true;
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
    }
}
