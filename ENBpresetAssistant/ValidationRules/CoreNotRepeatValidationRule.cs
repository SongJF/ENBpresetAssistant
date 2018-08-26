using ENBpresetAssistant.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ENBpresetAssistant.ValidationRules
{
    public class CoreNotRepeatValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace((value ?? "").ToString())) return new ValidationResult(false, LocalizedHelper.GetLocalizedString("Validation_Required", Data.ID.StrRes_Common));

            if (FileHelper.PathAvailableOrNot(Data.SettingsData.StoragePath + Data.ID.Dir_Core + "\\" + value.ToString())) return new ValidationResult(false, LocalizedHelper.GetLocalizedString("Validation_Repeat", Data.ID.StrRes_Common));

            return ValidationResult.ValidResult;
        }
    }
}
