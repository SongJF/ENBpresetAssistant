using ENBpresetAssistant.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENBpresetAssistant.Pages.InstallWin.PresetAdd
{
    public class PresetInstall_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        private string _PresetName;
        private string _ContainedCoreVersion;

        public string PresetName
        {
            get { return _PresetName; }
            set { this.MutateVerbose(ref _PresetName, value, RaisePropertyChanged()); }
        }

        public string ContainedCoreVersion
        {
            get { return _ContainedCoreVersion; }
            set { this.MutateVerbose(ref _ContainedCoreVersion, value, RaisePropertyChanged()); }
        }
    }
}
