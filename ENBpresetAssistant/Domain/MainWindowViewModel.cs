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
                new MenuList(Tools.LocalizedHelper.GetLocalizedString("EnbPresets","MainStr"),new EnbPresets()),
                new MenuList(Tools.LocalizedHelper.GetLocalizedString("EnbCores","MainStr"),new EnbCores()),
                new MenuList(Tools.LocalizedHelper.GetLocalizedString("Settings","MainStr"),new Settings())
            };
        }
    }
}
