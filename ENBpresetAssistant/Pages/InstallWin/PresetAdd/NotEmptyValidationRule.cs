using ENBpresetAssistant.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ENBpresetAssistant.Pages.InstallWin.PresetAdd
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, LocalizedHelper.GetLocalizedString("Validation_Required", Data.ID.StrRes_Common))
                : ValidationResult.ValidResult;
        }
    }
}
