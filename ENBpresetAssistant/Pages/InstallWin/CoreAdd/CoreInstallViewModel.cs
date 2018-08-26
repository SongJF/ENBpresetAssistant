using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENBpresetAssistant.Domain;

namespace ENBpresetAssistant.Pages.InstallWin.CoreAdd
{
    public class CoreInstallViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        private string _CoreVersion;
        public string CoreVersion
        {
            get { return _CoreVersion; }
            set { this.MutateVerbose(ref _CoreVersion, value, RaisePropertyChanged()); }
        }
    }
}
