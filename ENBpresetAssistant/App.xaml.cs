using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ENBpresetAssistant
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(true, "OnlyRun_CRNS");
           
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("An instance of ENBpresetAssistant is already running！","ENBpresetAssistant",MessageBoxButton.OK,MessageBoxImage.Information,);
                this.Shutdown();
            }
            base.OnStartup(e);
        }
    }
}
