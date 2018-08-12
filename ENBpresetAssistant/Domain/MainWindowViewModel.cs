using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ENBpresetAssistant.Pages;
using MaterialDesignThemes.Wpf;

namespace ENBpresetAssistant.Domain
{
    class MainWindowViewModel
    {
        public MenuList[] MenuLists { get; }

        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
        {
            if (snackbarMessageQueue == null) throw new ArgumentNullException(nameof(snackbarMessageQueue));

            MenuLists = new[]
            {
                new MenuList("EnbPresets",new EnbPresets()),
                new MenuList("EnbCores",new EnbCores()),
                new MenuList("Settings",new Settings())
            };
        }
    }
}
