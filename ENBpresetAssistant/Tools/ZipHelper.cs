using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBpresetAssistant.Tools
{
    class ZipHelper
    {
        public static bool Unzip(string ZipFile,string UnzipPath)
        {
            if (String.IsNullOrEmpty(ZipFile)) throw new ArgumentNullException("Empty Zip File Path");

            if (!File.Exists(ZipFile)) throw new FileNotFoundException("ZipFile Not Found");

            try
            {
                FastZip fastZip = new FastZip();
                fastZip.ExtractZip(ZipFile, UnzipPath, String.Empty);
            }
            catch(Exception e)
            {
                throw e;
            }


            return true;
        }
    }
}
